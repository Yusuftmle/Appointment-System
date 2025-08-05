using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.RequestModels.BlogTag.Create
{
    public class CreateBlogTagCommand:IRequest<ResultModel<BlogTagDto>>
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }

        public Guid? AssignToPostId { get; set; }
    }
}
