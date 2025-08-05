using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.RequestModels.BlogTag.Update
{
    public class UpdateBlogTagCommand:IRequest<Guid>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }    

    }
}
