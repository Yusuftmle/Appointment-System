using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Domain.Models;
using MediatR;

namespace Application.Queries.Blog
{
    public class GetAllBlogPostTaqQuery:IRequest<List<BlogTagDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
       

    }
}
