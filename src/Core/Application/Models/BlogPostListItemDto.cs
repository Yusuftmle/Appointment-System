
namespace Application.Queries.Blog
{
    public class BlogPostListItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string CoverImageUrl { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> TagNames { get; set; }

        public string Slug { get; set; }
    }
}