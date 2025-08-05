using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.RequestModels.BlogTag.Delete
{
    public class BlogTagDeleteCommand:IRequest<Guid>
    {
        public Guid Id { get; set; }    
    }
}
