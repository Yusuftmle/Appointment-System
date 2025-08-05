using System.Text;
using System.Xml.Linq;
using Application.Queries.Blog;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SitemapController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SitemapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("sitemap.xml")]
        public async Task<IActionResult> GetSitemap()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            // Statik URL'ler - bunlar Blazor WASM sayfaları
            var staticUrls = new List<SitemapUrl>
            {
                new SitemapUrl($"{baseUrl}/", DateTime.UtcNow, "daily", 1.0),
                new SitemapUrl($"{baseUrl}/hakkimda", DateTime.UtcNow.AddDays(-10), "monthly", 0.7),
                new SitemapUrl($"{baseUrl}/ozgecmis", DateTime.UtcNow.AddDays(-20), "monthly", 0.6),
                new SitemapUrl($"{baseUrl}/uzmanliklarimiz", DateTime.UtcNow.AddDays(-15), "monthly", 0.6),
                new SitemapUrl($"{baseUrl}/rezervasyon", DateTime.UtcNow.AddDays(-5), "weekly", 0.8),
                new SitemapUrl($"{baseUrl}/blog", DateTime.UtcNow.AddDays(-1), "daily", 0.9),
                new SitemapUrl($"{baseUrl}/contact", DateTime.UtcNow.AddDays(-10), "monthly", 0.7)
            };

            var blogs = await _mediator.Send(new GetAllBlogsQuery { OnlyPublished = true });
            var sitemapUrls = new List<SitemapUrl>(staticUrls);

            // Blog URL'lerini ekle - API domain'inde statik HTML dosyaları
            sitemapUrls.AddRange(blogs
                .Where(blog => !string.IsNullOrWhiteSpace(blog.Slug))
                .Select(blog =>
                    new SitemapUrl($"{Request.Scheme}://{Request.Host}/blog/{blog.Slug}.html", // API domain
                                  blog.UpdateDate > DateTime.MinValue ? blog.UpdateDate.Value : blog.CreatedAt,
                                  "weekly",
                                  0.8)));

            // XML oluştur
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var urlset = new XElement(ns + "urlset",
                sitemapUrls.Select(url =>
                    new XElement(ns + "url",
                        new XElement(ns + "loc", url.Url),
                        new XElement(ns + "lastmod", url.LastModified.ToString("yyyy-MM-dd")),
                        new XElement(ns + "changefreq", url.ChangeFreq),
                        new XElement(ns + "priority", url.Priority.ToString("F1", System.Globalization.CultureInfo.InvariantCulture))
                    )
                )
            );

            var xmlDoc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), urlset);
            return Content(xmlDoc.ToString(), "application/xml", Encoding.UTF8);
        }
    }

    public class SitemapUrl
    {
        public string Url { get; }
        public DateTime LastModified { get; }
        public string ChangeFreq { get; }
        public double Priority { get; }

        public SitemapUrl(string url, DateTime lastModified, string changeFreq, double priority)
        {
            Url = url;
            LastModified = lastModified;
            ChangeFreq = changeFreq;
            Priority = priority;
        }
    }
}