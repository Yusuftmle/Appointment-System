using Blazored.LocalStorage;
using Blazored.Modal;
using HotelVR.WebApp.Infrastructure.Auth;
using HotelVR.WebApp.Infrastructure.Services;
using HotelVR.WebApp.Infrastructure.Services.Interfaceses;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor.Services;
using System.Net.Http;
using Microsoft.JSInterop;
using System.Globalization;

namespace HotelVR.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            // Prerender konfigürasyonu
          
            builder.Services.AddTransient<AuthTokenHandler>();

            builder.Services.AddHttpClient("API", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5000");
            })
            .AddHttpMessageHandler<AuthTokenHandler>();

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("tr-TR");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("tr-TR");

            builder.Services.AddScoped<IidentityService, identityService>();
            builder.Services.AddSingleton(sp => (IJSInProcessRuntime)sp.GetRequiredService<IJSRuntime>());

            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<UploadService>();

            builder.Services.AddMudServices();

            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            builder.Services.AddScoped<IBlogPostService, BlogPostService>();

            builder.Services.AddSingleton(sp =>
                new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/notifications")
                    .WithAutomaticReconnect()
                    .Build());

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredModal();

            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();


        }
    }
}
