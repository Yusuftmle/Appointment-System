using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Queries.Blog
{
    public class GetBlogPostTagIdQuery:IRequest<BlogTagDto>
    {
        public GetBlogPostTagIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
