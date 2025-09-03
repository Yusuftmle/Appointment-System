using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Repositories.Interfaces;
using AutoMapper;
using Domain.Models;
using HotelRv.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Application.Queries.Blog
{
        public class GetAllBlogPostTaqQueryHandler : IRequestHandler<GetAllBlogPostTaqQuery, List<BlogTagDto>>
        {
            private readonly IBlogTagRepository _blogPostTagRepository;
           

            public GetAllBlogPostTaqQueryHandler(IBlogTagRepository blogPostTagRepository)
            {
                _blogPostTagRepository = blogPostTagRepository;
               
            }

            public async Task<List<BlogTagDto>> Handle(GetAllBlogPostTaqQuery request, CancellationToken cancellationToken)
            {
            //  AsQueryable kullanarak projection
            return await _blogPostTagRepository.AsQueryable()
                .AsNoTracking()
                .Where(t => t.IsDeleted) //  Filtering eklenebilir
                .OrderBy(t => t.Name)   //  Sorting eklenebilir
                .Select(t => new BlogTagDto //  Database-level projection
                {
                    Id = t.Id,
                    Name = t.Name,
                  
                })
                .ToListAsync(cancellationToken); //  CancellationToken support
        }
        }
}
