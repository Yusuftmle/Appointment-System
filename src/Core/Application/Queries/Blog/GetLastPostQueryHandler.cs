using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Page;
using static Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;
using Application.Repositories.Interfaces;
using MediatR;
using Application.Extension;
using Application.Queries.Blog;

namespace Application.Queries
{
    public class GetLastPostQueryHandler : IRequestHandler<GetLastPostQuery, PagedViewModel<BlogPostListItemDto>>
    {
        private readonly IBlogPostRepository blogPostRepository;

        public GetLastPostQueryHandler(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }

        public async Task<PagedViewModel<BlogPostListItemDto>> Handle(GetLastPostQuery request, CancellationToken cancellationToken)
        {
            var query = blogPostRepository.AsQueryable()
        .AsNoTracking()
        .Where(x => !x.IsDeleted)
        .OrderByDescending(x => x.CreateDate);

            var list = query.Select(i => new BlogPostListItemDto()
            {
                Id = i.Id,
                Title = i.Title,
                Summary = i.Summary,
                CoverImageUrl = i.CoverImageUrl,
                Author = i.User.FullName,
                CreatedAt = i.CreateDate,
                TagNames = i.BlogPostTags.Select(t => t.BlogTag.Name).ToList(),
                Slug = i.Slug

            });

            var entries = await list.GetPaged(request.Page, request.pageSize);
            return entries;


        }


    }
    
}
