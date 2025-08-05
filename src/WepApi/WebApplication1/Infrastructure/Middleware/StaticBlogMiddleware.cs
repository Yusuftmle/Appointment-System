using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HotelApi.Infrastructure.Middleware
{
    public class StaticBlogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<StaticBlogMiddleware> _logger;

        // Bot User-Agent'ları
        private readonly string[] _botUserAgents = {
            "googlebot", "bingbot", "slurp", "duckduckbot", "baiduspider",
            "yandexbot", "facebookexternalhit", "twitterbot", "linkedinbot",
            "whatsapp", "telegrambot", "applebot", "crawler", "spider"
        };

        public StaticBlogMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<StaticBlogMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();

            // Blog path kontrolü
            if (!string.IsNullOrWhiteSpace(path) && path.StartsWith("/blog") && !path.EndsWith(".html"))
            {
                try
                {
                    // User-Agent kontrolü
                    var userAgent = context.Request.Headers["User-Agent"].ToString().ToLowerInvariant();
                    var isBot = IsBot(userAgent);

                    // Sadece bot'lar için statik HTML serve et
                    if (isBot)
                    {
                        var handled = await HandleBotRequest(context, path);
                        if (handled) return;
                    }
                    else
                    {
                        // Normal kullanıcılar için SPA'ya yönlendir (optional log)
                        _logger.LogInformation("Blog request from human user: {Path}, forwarding to SPA", path);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in StaticBlogMiddleware for path: {Path}", path);
                }
            }

            await _next(context);
        }

        private bool IsBot(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return false;

            return _botUserAgents.Any(bot => userAgent.Contains(bot));
        }

        private async Task<bool> HandleBotRequest(HttpContext context, string path)
        {
            // URL'den slug çıkar
            var pathSegments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (pathSegments.Length < 2)
            {
                _logger.LogWarning("Invalid blog path format: {Path}", path);
                return false;
            }

            var slug = pathSegments.Last();

            // Güvenlik: Path traversal saldırılarını önle
            if (IsInvalidSlug(slug))
            {
                _logger.LogWarning("Invalid slug detected: {Slug} from IP: {IP}",
                    slug, GetClientIP(context));
                return false;
            }

            // Statik dosya yolu
            var staticFilePath = Path.Combine(_env.WebRootPath, "blog-static", $"{slug}.html");

            if (File.Exists(staticFilePath))
            {
                try
                {
                    // Cache headers ekle
                    var lastModified = File.GetLastWriteTimeUtc(staticFilePath);
                    context.Response.Headers.Add("Cache-Control", "public, max-age=3600");
                    context.Response.Headers.Add("Last-Modified", lastModified.ToString("R"));

                    // ETag ekle
                    var etag = $"\"{lastModified.Ticks:x}\"";
                    context.Response.Headers.Add("ETag", etag);

                    // If-None-Match kontrolü
                    var clientETag = context.Request.Headers["If-None-Match"].ToString();
                    if (clientETag == etag)
                    {
                        context.Response.StatusCode = 304; // Not Modified
                        return true;
                    }

                    // SEO meta bilgileri için content type
                    context.Response.ContentType = "text/html; charset=utf-8";

                    // Dosyayı serve et
                    await context.Response.SendFileAsync(staticFilePath);

                    _logger.LogInformation("Served static blog content: {Slug} to bot: {UserAgent}",
                        slug, context.Request.Headers["User-Agent"].ToString());

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error serving static file: {FilePath}", staticFilePath);
                    return false;
                }
            }
            else
            {
                _logger.LogWarning("Static blog file not found: {FilePath} for slug: {Slug}",
                    staticFilePath, slug);
                return false;
            }
        }

        private bool IsInvalidSlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return true;

            // Güvenlik kontrolleri
            var invalidChars = new[] { "..", "/", "\\", "<", ">", ":", "*", "?", "\"", "|" };
            return invalidChars.Any(slug.Contains);
        }

        private string GetClientIP(HttpContext context)
        {
            // Proxy arkasındaysa gerçek IP'yi al
            var forwarded = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwarded))
            {
                return forwarded.Split(',')[0].Trim();
            }

            var realIP = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIP))
            {
                return realIP;
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }
    }

   
}