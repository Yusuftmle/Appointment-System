using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.RequestModels.BlogPost.DeleteBlog
{
    public class DeleteBlogPostCommand:IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}
