using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Extension;
using Application.Models;
using Application.Models.Page;
using Application.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;
using AutoMapper;
using MediatR;

namespace Application.Queries.Blog
{
    public class GetPaginatedBlogPostsQueryHandler: IRequestHandler<GetPaginatedBlogPostsQuery, PagedViewModel<BlogPostDto>>
    {
        private readonly IBlogPostRepository blogPostRepository;
        

        public GetPaginatedBlogPostsQueryHandler(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
          
        }

        public async Task<PagedViewModel<BlogPostDto>> Handle(GetPaginatedBlogPostsQuery request, CancellationToken cancellationToken)
        {
            var query = blogPostRepository.AsQueryable();

            query = query
                .AsNoTracking();
             

            var list = query.Select(i => new BlogPostDto()
            {
                Id = i.Id,
                BlogTagId = i.BlogTagId,
                Summary = i.Summary,
                Title = i.Title,
                UserId = i.UserId,
                Content = i.Content,
                Slug = i.Slug,
                CoverImageUrl = i.CoverImageUrl,
                Author = i.User.FullName,
                CreatedAt = i.CreatedAt,
                Keywords = i.Keywords,
                ViewCount = i.ViewCount,
                BlogTagName = i.BlogPostTags.Select(pt => pt.BlogTag.Name).FirstOrDefault() ?? "No Tag" // Güvenli erişim
            });

            var entries = await list.GetPaged(request.Page, request.pageSize);
            return entries;
        }
    }
}
