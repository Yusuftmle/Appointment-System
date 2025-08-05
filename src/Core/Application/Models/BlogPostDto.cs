using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class BlogPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string CoverImageUrl { get; set; }
        public bool IsPublished { get; set; }
        public int ViewCount { get; set; }
        public string Keywords { get; set; }
        public string BlogTagName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid UserId { get; set; }
        public Guid BlogTagId { get; set; }
        public List<BlogTagDto> BlogTags { get; set; }
    }
}
