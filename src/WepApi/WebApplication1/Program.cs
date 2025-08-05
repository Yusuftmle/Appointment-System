using Application.Extension;
using FluentValidation;
using FluentValidation.AspNetCore;
using HotelApi.Infrastructure;
using HotelApi.Infrastructure.SignalR;
using Microsoft.AspNetCore.ResponseCompression;
using HotelRvDbContext.Infrastructure.Persistence.Extensions;
using Microsoft.OpenApi.Models;
using HotelRv.Infrastructure.Persistence.Extensions;
using HotelApi.Infrastructure.Middleware;


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
