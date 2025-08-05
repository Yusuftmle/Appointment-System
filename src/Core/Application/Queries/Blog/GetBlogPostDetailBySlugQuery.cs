using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Queries.Blog
{
    public class GetBlogPostDetailBySlugQuery : IRequest<BlogPostDto>
    {
        public string Slug { get; }

        public GetBlogPostDetailBySlugQuery(string slug)
        {
            Slug = slug;
        }
    }
}
