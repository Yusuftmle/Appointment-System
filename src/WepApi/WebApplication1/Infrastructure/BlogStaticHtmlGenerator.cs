using Application.Models;
using Application.RequestModels.BlogPost.CreateBlog;
using System.Globalization;
using System.Text;

namespace HotelApi.Infrastructure
{
    public class BlogStaticHtmlGenerator : IBlogStaticHtmlGenerator
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public BlogStaticHtmlGenerator(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        public async Task GenerateAsync(CreateBlogPostCommand blog)
        {
          
            var clientBaseUrl = _config["SeoSettings:ClientBaseUrl"];
            var siteTitle = _config["SeoSettings:SiteTitle"];
            var canonicalUrl = $"{clientBaseUrl}/blog/{blog.Slug}";
            var templatePath = Path.Combine(_env.ContentRootPath, "Infrastructure", "StaticFiles", "Templates", "BlogHtmlTemplate.html");
            var outputDir = Path.Combine(_env.WebRootPath, "blog");

            // Slug kullan veya benzersiz bir dosya adı oluştur
            var fileName = !string.IsNullOrWhiteSpace(blog.Slug)
                ? $"{blog.Slug}.html"
                : $"{Guid.NewGuid()}.html";

            var outputPath = Path.Combine(outputDir, fileName);

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            var template = await File.ReadAllTextAsync(templatePath, Encoding.UTF8);
            var filledHtml = template
                .Replace("{{CanonicalUrl}}", canonicalUrl)
                .Replace("{{Title}}", blog.Title ?? "")
                .Replace("{{Summary}}", blog.Summary ?? "")
                .Replace("{{Keywords}}", blog.Keywords ?? "")
                .Replace("{{Slug}}", blog.Slug ?? "")
                .Replace("{{Author}}", blog.Author ?? "")
                .Replace("{{CoverImageUrl}}", blog.CoverImageUrl ?? "")
                .Replace("{{CreatedAt}}", blog.CreatedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                .Replace("{{Content}}", blog.Content ?? "")
             .Replace("{{SiteTitle}}", siteTitle);

            await File.WriteAllTextAsync(outputPath, filledHtml, Encoding.UTF8);
        }


    }
}
