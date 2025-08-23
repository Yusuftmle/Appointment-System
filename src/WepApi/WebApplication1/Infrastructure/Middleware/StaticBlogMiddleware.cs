using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using Application.Models.BlogMiddelewareModel;

namespace HotelApi.Infrastructure.Middleware
{
    public class StaticBlogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<StaticBlogMiddleware> _logger;
        private readonly IConfiguration _config;

        // Comprehensive Bot User-Agents (updated 2024)
        private readonly HashSet<string> _botUserAgents = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // Search Engine Bots
            "googlebot", "bingbot", "slurp", "duckduckbot", "baiduspider", "yandexbot",
            "sogou", "exabot", "facebot", "ia_archiver",
            
            // Social Media Crawlers
            "facebookexternalhit", "twitterbot", "linkedinbot", "pinterest", "whatsapp",
            "telegrambot", "skypeuripreview", "slackbot", "discordbot",
            
            // SEO & Analytics
            "ahrefsbot", "semrushbot", "mj12bot", "dotbot", "rogerbot", "screaming frog",
            "sitebulb", "deepcrawl", "botify", "oncrawl",
            
            // AI & Content Crawlers  
            "gptbot", "google-extended", "ccbot", "anthropic-ai", "claude-web",
            "openai", "chatgpt-user", "bard", "perplexity",
            
            // General Crawlers
            "applebot", "crawler", "spider", "scraper", "bot", "archiver",
            "fetch", "wget", "curl", "libwww", "lwp", "httrack", "python-requests",
            "okhttp", "java", "apache-httpclient", "node-fetch", "axios",

            "postmanruntime", "postman", "newman", // Postman varyantları
           "insomnia", "paw", "httpie", "rest-client", // Diğer REST istemcileri
           "thunder client", "rapidapi", "restlet" // Ek API test araçları
        };

        // Known Bot IP Ranges (Major search engines)
        private readonly Dictionary<string, string[]> _botIpRanges = new Dictionary<string, string[]>
        {
            ["Google"] = new[] { "66.249.", "64.233.", "72.14.", "74.125.", "209.85.", "216.239." },
            ["Bing"] = new[] { "157.55.", "157.56.", "199.30.", "207.46.", "40.77.", "40.76." },
            ["Yandex"] = new[] { "77.88.", "87.250.", "95.108.", "178.154.", "199.21." },
            ["Baidu"] = new[] { "180.76.", "123.125.", "220.181." },
            ["Facebook"] = new[] { "31.13.", "66.220.", "69.63.", "69.171.", "74.119.", "173.252." },
            ["Twitter"] = new[] { "199.16.", "199.96." }
        };

        // Suspicious patterns that might indicate crawlers
        private readonly string[] _crawlerPatterns = new[]
        {
            @"bot\b", @"crawl", @"spider", @"scrape", @"fetch", @"index",
            @"http", @"www", @"url", @"link", @"site", @"page"
        };

        // Configuration keys
        private const string ENABLE_IP_DETECTION = "Blog:EnableIpDetection";
        private const string ENABLE_PATTERN_DETECTION = "Blog:EnablePatternDetection";
        private const string STATIC_CACHE_HOURS = "Blog:StaticCacheHours";

        public StaticBlogMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<StaticBlogMiddleware> logger, IConfiguration config)
        {
            _next = next;
            _env = env;
            _logger = logger;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant();

            // Blog path kontrolü
            if (!string.IsNullOrWhiteSpace(path) && path.StartsWith("/blog") && !path.EndsWith(".html"))
            {
                var startTime = DateTime.UtcNow;
                var clientInfo = ExtractClientInfo(context);

                try
                {
                    _logger.LogInformation("Blog request received | Path: {Path} | IP: {IP} | UserAgent: {UserAgent} | Referrer: {Referrer}",
                        path, clientInfo.IpAddress, clientInfo.UserAgent, clientInfo.Referrer);

                    // Enhanced bot detection
                    var botDetectionResult = await DetectBot(context, clientInfo);

                    if (botDetectionResult.IsBot)
                    {
                        _logger.LogInformation("Bot detected | Type: {BotType} | Method: {DetectionMethod} | Confidence: {Confidence}% | Path: {Path}",
                            botDetectionResult.BotType, botDetectionResult.DetectionMethod, botDetectionResult.Confidence, path);

                        var handled = await HandleBotRequest(context, path, botDetectionResult);

                        var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                        _logger.LogInformation("Bot request processed | Handled: {Handled} | Duration: {Duration}ms | Path: {Path}",
                            handled, duration, path);

                        if (handled) return;
                    }
                    else
                    {
                        _logger.LogInformation("Human user detected | Path: {Path} | Detection Score: {Score} | Forwarding to reverse proxy fallback",
                            path, botDetectionResult.Confidence);

                        // Set header for reverse proxy to handle
                        context.Response.Headers.Add("X-Human-Request", "true");
                        context.Response.StatusCode = 404; // Let reverse proxy handle fallback
                        return;
                    }
                }
                catch (Exception ex)
                {
                    var duration = (DateTime.UtcNow - startTime).TotalMilliseconds;
                    _logger.LogError(ex, "Critical error in StaticBlogMiddleware | Path: {Path} | IP: {IP} | Duration: {Duration}ms | Error: {ErrorMessage}",
                        path, clientInfo.IpAddress, duration, ex.Message);

                    // Fail-safe: Let request continue to avoid breaking the site
                    await _next(context);
                    return;
                }
            }

            await _next(context);
        }

        private ClientInfo ExtractClientInfo(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var referrer = context.Request.Headers["Referer"].ToString();
            var acceptHeader = context.Request.Headers["Accept"].ToString();
            var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();
            var acceptEncoding = context.Request.Headers["Accept-Encoding"].ToString();

            return new ClientInfo
            {
                IpAddress = GetClientIP(context),
                UserAgent = userAgent,
                Referrer = referrer,
                AcceptHeader = acceptHeader,
                AcceptLanguage = acceptLanguage,
                AcceptEncoding = acceptEncoding,
                RequestTime = DateTime.UtcNow,
                HasJavaScriptCapability = acceptHeader.Contains("text/html") && !string.IsNullOrEmpty(acceptLanguage)
            };
        }

        private async Task<BotDetectionResult> DetectBot(HttpContext context, ClientInfo clientInfo)
        {
            var result = new BotDetectionResult();
            var detectionMethods = new List<string>();
            var confidenceScore = 0;

            // 1. User-Agent Detection (40% weight)
            var userAgentResult = DetectBotByUserAgent(clientInfo.UserAgent);
            if (userAgentResult.IsDetected)
            {
                result.BotType = userAgentResult.BotType;
                confidenceScore += 40;
                detectionMethods.Add("UserAgent");
            }

            // 2. IP Range Detection (30% weight) 
            if (_config.GetValue<bool>(ENABLE_IP_DETECTION, true))
            {
                var ipResult = DetectBotByIP(clientInfo.IpAddress);
                if (ipResult.IsDetected)
                {
                    if (string.IsNullOrEmpty(result.BotType))
                        result.BotType = ipResult.BotType;
                    confidenceScore += 30;
                    detectionMethods.Add("IP");
                }
            }

            // 3. Request Pattern Analysis (20% weight)
            if (_config.GetValue<bool>(ENABLE_PATTERN_DETECTION, true))
            {
                var patternResult = AnalyzeRequestPattern(clientInfo);
                if (patternResult.IsBot)
                {
                    confidenceScore += 20;
                    detectionMethods.Add("Pattern");
                }
            }

            // 4. Header Analysis (10% weight)
            var headerResult = AnalyzeHeaders(clientInfo);
            if (headerResult.IsSuspicious)
            {
                confidenceScore += 10;
                detectionMethods.Add("Header");
            }

            result.IsBot = confidenceScore >= 50; // 50% threshold
            result.Confidence = Math.Min(confidenceScore, 100);
            result.DetectionMethod = string.Join(", ", detectionMethods);

            // Default to "Unknown" if bot detected but type not identified
            if (result.IsBot && string.IsNullOrEmpty(result.BotType))
                result.BotType = "Unknown Bot";

            // Log detailed detection info
            if (result.IsBot)
            {
                _logger.LogDebug("Bot detection details | IP: {IP} | UA: {UserAgent} | Score: {Score} | Methods: {Methods}",
                    clientInfo.IpAddress, clientInfo.UserAgent, result.Confidence, result.DetectionMethod);
            }

            return result;
        }

        private UserAgentResult DetectBotByUserAgent(string userAgent)
        {
            if (string.IsNullOrWhiteSpace(userAgent))
                return new UserAgentResult { IsDetected = true, BotType = "Empty UserAgent" };

            var lowerUserAgent = userAgent.ToLowerInvariant();

            // Direct match with known bots
            foreach (var botAgent in _botUserAgents)
            {
                if (lowerUserAgent.Contains(botAgent))
                {
                    return new UserAgentResult
                    {
                        IsDetected = true,
                        BotType = CapitalizeBotName(botAgent)
                    };
                }
            }

            // Pattern matching for sophisticated bots
            var botPatterns = new[]
            {
                @"bot[\s/]", @"crawl", @"spider", @"scraper", @"fetch", @"search",
                @"monitor", @"check", @"test", @"validator", @"analyzer"
            };

            foreach (var pattern in botPatterns)
            {
                if (Regex.IsMatch(lowerUserAgent, pattern, RegexOptions.IgnoreCase))
                {
                    return new UserAgentResult
                    {
                        IsDetected = true,
                        BotType = "Pattern Matched Bot"
                    };
                }
            }

            return new UserAgentResult { IsDetected = false };
        }

        private IPResult DetectBotByIP(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return new IPResult { IsDetected = false };

            foreach (var botProvider in _botIpRanges)
            {
                foreach (var ipPrefix in botProvider.Value)
                {
                    if (ipAddress.StartsWith(ipPrefix))
                    {
                        return new IPResult
                        {
                            IsDetected = true,
                            BotType = botProvider.Key + " Bot"
                        };
                    }
                }
            }

            return new IPResult { IsDetected = false };
        }

        private PatternResult AnalyzeRequestPattern(ClientInfo clientInfo)
        {
            var suspiciousScore = 0;

            // Missing common browser headers
            if (string.IsNullOrEmpty(clientInfo.AcceptLanguage))
                suspiciousScore += 25;

            if (string.IsNullOrEmpty(clientInfo.AcceptEncoding))
                suspiciousScore += 20;

            // Unusual Accept header
            if (!clientInfo.AcceptHeader.Contains("text/html"))
                suspiciousScore += 30;

            // Very short or very long User-Agent
            var uaLength = clientInfo.UserAgent?.Length ?? 0;
            if (uaLength < 20 || uaLength > 500)
                suspiciousScore += 25;

            return new PatternResult { IsBot = suspiciousScore >= 50 };
        }

        private HeaderResult AnalyzeHeaders(ClientInfo clientInfo)
        {
            var suspiciousScore = 0;

            // Check for automation tools
            var automationTools = new[] { "curl", "wget", "postman", "insomnia", "httpie" };
            var lowerUA = clientInfo.UserAgent?.ToLowerInvariant() ?? "";

            if (automationTools.Any(tool => lowerUA.Contains(tool)))
                suspiciousScore += 50;

            // Missing referrer (not always suspicious, but can be)
            if (string.IsNullOrEmpty(clientInfo.Referrer))
                suspiciousScore += 10;

            return new HeaderResult { IsSuspicious = suspiciousScore >= 30 };
        }

        private async Task<bool> HandleBotRequest(HttpContext context, string path, BotDetectionResult botInfo)
        {
            var pathSegments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (pathSegments.Length < 2)
            {
                _logger.LogWarning("Invalid blog path format detected | Path: {Path} | Bot: {BotType} | IP: {IP}",
                    path, botInfo.BotType, GetClientIP(context));
                return false;
            }

            var slug = pathSegments.Last();

            if (IsInvalidSlug(slug))
            {
                _logger.LogWarning("Security: Invalid slug detected | Slug: {Slug} | Bot: {BotType} | IP: {IP} | UserAgent: {UserAgent}",
                    slug, botInfo.BotType, GetClientIP(context), context.Request.Headers["User-Agent"].ToString());
                return false;
            }

            var staticFilePath = Path.Combine(_env.WebRootPath, "blog-static", $"{slug}.html");

            if (!File.Exists(staticFilePath))
            {
                _logger.LogWarning("Static blog file not found | Path: {FilePath} | Slug: {Slug} | Bot: {BotType} | IP: {IP}",
                    staticFilePath, slug, botInfo.BotType, GetClientIP(context));
                return false;
            }

            try
            {
                var fileInfo = new FileInfo(staticFilePath);
                var lastModified = fileInfo.LastWriteTimeUtc;
                var fileSize = fileInfo.Length;

                // Enhanced caching with configurable duration
                var cacheHours = _config.GetValue<int>(STATIC_CACHE_HOURS, 24);
                context.Response.Headers.Add("Cache-Control", $"public, max-age={cacheHours * 3600}");
                context.Response.Headers.Add("Last-Modified", lastModified.ToString("R"));

                // Generate strong ETag with bot info
                var etagBase = $"{lastModified.Ticks}-{fileSize}-{botInfo.BotType}";
                var etag = $"\"{ComputeHash(etagBase)}\"";
                context.Response.Headers.Add("ETag", etag);

                // Enhanced 304 Not Modified support
                var clientETag = context.Request.Headers["If-None-Match"].ToString();
                var ifModifiedSince = context.Request.Headers["If-Modified-Since"].ToString();

                if ((clientETag == etag) ||
                    (!string.IsNullOrEmpty(ifModifiedSince) && DateTime.TryParse(ifModifiedSince, out var clientDate) && clientDate >= lastModified))
                {
                    context.Response.StatusCode = 304;
                    _logger.LogInformation("304 Not Modified returned | Bot: {BotType} | Slug: {Slug} | ETag: {ETag}",
                        botInfo.BotType, slug, etag);
                    return true;
                }

                // SEO and bot-friendly headers
                context.Response.ContentType = "text/html; charset=utf-8";
                context.Response.Headers.Add("X-Robots-Tag", "index, follow");
               context.Response.Headers.Add("X-Bot-Served", 
    Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(botInfo.BotType)));



                // Serve the file
                await context.Response.SendFileAsync(staticFilePath);

                _logger.LogInformation("Static blog served successfully | Bot: {BotType} | Confidence: {Confidence}% | Slug: {Slug} | FileSize: {FileSize} bytes | CacheHit: {CacheHit}",
                    botInfo.BotType, botInfo.Confidence, slug, fileSize, clientETag == etag ? "Yes" : "No");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error serving static file | Path: {FilePath} | Bot: {BotType} | Error: {ErrorMessage}",
                    staticFilePath, botInfo.BotType, ex.Message);
                return false;
            }
        }
      

        private bool IsInvalidSlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                return true;

            // Enhanced security checks
            var invalidChars = new[] { "..", "/", "\\", "<", ">", ":", "*", "?", "\"", "|", ";", "&", "$", "%", "#" };
            if (invalidChars.Any(slug.Contains))
                return true;

            // Check for SQL injection patterns
            var sqlPatterns = new[] { "'", "union", "select", "drop", "insert", "update", "delete" };
            var lowerSlug = slug.ToLowerInvariant();
            if (sqlPatterns.Any(lowerSlug.Contains))
                return true;

            // Length check
            if (slug.Length > 100)
                return true;

            return false;
        }

        private string GetClientIP(HttpContext context)
        {
            // Enhanced IP extraction with multiple fallbacks
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                var ip = forwardedFor.Split(',')[0].Trim();
                if (IPAddress.TryParse(ip, out _))
                    return ip;
            }

            var realIP = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(realIP) && IPAddress.TryParse(realIP, out _))
                return realIP;

            var cfConnectingIP = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault();
            if (!string.IsNullOrEmpty(cfConnectingIP) && IPAddress.TryParse(cfConnectingIP, out _))
                return cfConnectingIP;

            return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        private string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hash)[..16]; // First 16 chars
            }
        }

        private string CapitalizeBotName(string botName)
        {
            if (string.IsNullOrEmpty(botName))
                return "Unknown Bot";

            return char.ToUpper(botName[0]) + botName.Substring(1).ToLower();
        }
    }

    // Supporting classes
  

   

    

   

  

}