using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.RequestModels.BlogPost.UpdateBlog
{
    public class UpdateBlogPostComamnd:IRequest<Guid>
    {
        public Guid Id { get; set; }                     // Güncellemek için gerekli
        public string Title { get; set; }                // Güncellenebilir
        public string Content { get; set; }              // Güncellenebilir
        public bool IsActive { get; set; }   
        // Opsiyonel ama yayında mı değil mi anlamak için kullanılabilir
        public string Summary { get; set; }
        // Tag güncellenebilir
       public Guid BlogTagId { get; set; }

        public string CoverImageUrl { get; set; }
        // Güncellenebilir
        public string Keywords { get; set; }


    }
}
