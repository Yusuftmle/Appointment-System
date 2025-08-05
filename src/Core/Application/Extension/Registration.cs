using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Domain.Models;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
namespace Application.Extension
{
    public static class Registration
    {
        public static IServiceCollection AddAplicationRegistration(this IServiceCollection services)
        {

            var assm = Assembly.GetExecutingAssembly();
            // Identity konfigürasyonu
            // Identity konfigürasyonu
          //  services.AddIdentity<User, IdentityRole>()
            //        .AddEntityFrameworkStores<DbContext> ()
           //     .AddDefaultTokenProviders();


            // MediatR konfigürasyonu düzeltildi

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assm));



            // AutoMapper konfigürasyonu
            services.AddAutoMapper(assm);


            // FluentValidation için doğrulayıcıları assembly'den ekleme
            services.AddValidatorsFromAssembly(assm);
           
            services.AddScoped<IHelpers, Helpers>();
            services.AddTransient<ImailService, mailService>();
            services.AddTransient<IGenerateSecureToken, GenerateSecureToken>();
            return services;
        }
    }
}
