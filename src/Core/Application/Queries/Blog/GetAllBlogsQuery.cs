using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Queries.Blog
{
    public class GetAllBlogsQuery:IRequest<List<BlogPostDto>>
    {
         
        public bool OnlyPublished { get; set; } = true;
    }
}
