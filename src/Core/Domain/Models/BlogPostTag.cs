using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class BlogPostTag:BaseEntity
    {
        public Guid BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        public Guid BlogTagId { get; set; }
        public BlogTag BlogTag { get; set; }
    }
}
