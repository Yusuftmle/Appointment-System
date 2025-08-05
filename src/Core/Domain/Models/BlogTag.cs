using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class BlogTag:BaseEntity
    {
        public string Name { get; set; }


        public ICollection<BlogPostTag> BlogPostTags { get; set; }
    }
}
