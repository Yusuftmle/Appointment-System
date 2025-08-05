using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class BlogPost:BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }

        public string? Slug { get; set; }

        public string Summary { get; set; }
        public string CoverImageUrl { get; set; }
        public int ViewCount { get; set; }
        public bool IsPublished { get; set; }
        public string Keywords { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid BlogTagId { get; set; }
        public ICollection<BlogPostTag> BlogPostTags { get; set; }

    }
}
