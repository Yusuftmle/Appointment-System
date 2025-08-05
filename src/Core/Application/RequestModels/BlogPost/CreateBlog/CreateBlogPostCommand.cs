using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;

namespace Application.RequestModels.BlogPost.CreateBlog
{
    public class CreateBlogPostCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string? Slug { get;  set; } // veya sadece getter

        public string Summary { get; set; }
        public string CoverImageUrl { get; set; }
        public int ViewCount { get; set; }
        public bool IsPublished { get; set; }
        public string Keywords { get; set; }
        public Guid BlogTagId { get; set; } = new();   
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
