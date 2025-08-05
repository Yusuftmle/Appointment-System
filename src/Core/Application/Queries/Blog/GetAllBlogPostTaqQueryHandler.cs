using System;
using System.Collections.Generic;
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
            private readonly IMapper mapper;

            public GetAllBlogPostTaqQueryHandler(IBlogTagRepository blogPostTagRepository, IMapper mapper)
            {
                _blogPostTagRepository = blogPostTagRepository;
                this.mapper = mapper;
            }

            public async Task<List<BlogTagDto>> Handle(GetAllBlogPostTaqQuery request, CancellationToken cancellationToken)
            {
                var Appointments = await _blogPostTagRepository.GetAll();
                return await Task.FromResult(mapper.Map<List<BlogTagDto>>(Appointments));
            }
        }
}
