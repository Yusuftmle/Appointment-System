using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Repositories.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Blog
{
    public class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQuery, List<BlogPostDto>>
    {
        private readonly IBlogPostRepository _blogRepository;
        private readonly IMapper _mapper;

        public GetAllBlogsQueryHandler(IBlogPostRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public async Task<List<BlogPostDto>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
        {
            var blogs = await _blogRepository.GetAll();

           
                blogs = blogs.Where(b => b.IsPublished).ToList();
            

            return _mapper.Map<List<BlogPostDto>>(blogs);
        }
    }
}
