using System.Threading.RateLimiting;
using Application.Extension;
using FluentValidation;
using FluentValidation.AspNetCore;
using HotelApi.Infrastructure;
using HotelApi.Infrastructure.Middleware;
using HotelApi.Infrastructure.SignalR;
using HotelRv.Infrastructure.Persistence.Extensions;
using HotelRvDbContext.Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;


namespace WebApplication1
{
    public class Program
    {
        private readonly IWebHostEnvironment _env;

        public Program(IWebHostEnvironment env)
        {
            _env = env;
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(opt => opt.Filters.Add<ValidateModelStateFilter>())
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .AddFluentValidation()
                .ConfigureApiBehaviorOptions(opt => opt.SuppressModelStateInvalidFilter = true);
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddHostedService<TokenCleanupService>();

            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            builder.Services.AddScoped<IBlogStaticHtmlGenerator, BlogStaticHtmlGenerator>();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAplicationRegistration();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel API", Version = "v1" });

                // Burada ekleyelim
                c.OperationFilter<FileUploadOperationFilter>();
            });
            builder.Services.AddInfrastructureRegistration(builder.Configuration);
            builder.Services.ConfigureAuth(builder.Configuration);
            builder.Services.AddSignalR();
            builder.Services.AddRateLimiter(options =>
            {
                // Global limit
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100, // 100 request
                            Window = TimeSpan.FromMinutes(1), // 1 dakikada
                            QueueLimit = 0
                        }));

                // Login için özel limit
                options.AddFixedWindowLimiter("loginLimiter", opt =>
                {
                    opt.PermitLimit = 5;
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.QueueLimit = 0;
                });
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    ["application/octet-stream"]);
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.ConfigureExceptionHandling(
                includeExceptionsDetails: builder.Environment.IsDevelopment(),
                 useDefaultHandlingResponse: true);
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRateLimiter();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            // Middleware'i ekle
            app.UseMiddleware<StaticBlogMiddleware>();
            app.MapHub<NotificationHub>("/notifications");
            app.UseStaticFiles(); // wwwroot'tan statik dosyalarý okuyabilmesi için
            app.MapControllers();
            app.UseResponseCompression();
            app.Run();
        }
    }
}
