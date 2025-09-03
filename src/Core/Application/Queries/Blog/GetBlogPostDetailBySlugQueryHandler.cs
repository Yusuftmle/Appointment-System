using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Repositories.Interfaces;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Blog
{
    public class GetBlogPostDetailBySlugQueryHandler : IRequestHandler<GetBlogPostDetailBySlugQuery, BlogPostDto>
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public GetBlogPostDetailBySlugQueryHandler(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<BlogPostDto> Handle(GetBlogPostDetailBySlugQuery request, CancellationToken cancellationToken)
        {
            var query = _blogPostRepository.AsQueryable();

            query = query
                .AsNoTracking()
                .Where(i => i.Slug == request.Slug && !i.IsDeleted); // filtre önemli!

            var result = await query.Select(i => new BlogPostDto()
            {
                Id=i.Id,
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
                BlogTags = i.BlogPostTags.Select(bpt => new BlogTagDto
                {
                    Id = bpt.BlogTag.Id,
                    Name = bpt.BlogTag.Name
                }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                throw new DataBaseValidationException("Blog bulunamadı");

            return result;
        }
    }
}
