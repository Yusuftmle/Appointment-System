using Application.Queries.user;
using Application.RequestModels.User.LoginCommand;
using Blazored.LocalStorage;
using HotelVR.Common.Infrastructure.Exceptions;
using HotelVR.Common.Results;
using HotelVR.WebApp.Infrastructure.Auth;
using HotelVR.WebApp.Infrastructure.Extensions;
using HotelVR.WebApp.Infrastructure.Services.Interfaceses;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json;

public class identityService : IidentityService
{
    private readonly HttpClient httpClient;
    private readonly ISyncLocalStorageService syncLocalStorageService;
    private readonly AuthenticationStateProvider authenticationStateProvider;

    public identityService(IHttpClientFactory httpClientFactory,
                           ISyncLocalStorageService syncLocalStorageService,
                           AuthenticationStateProvider authenticationStateProvider)
    {
        this.httpClient = httpClientFactory.CreateClient("API"); // tokenlı client
        this.syncLocalStorageService = syncLocalStorageService;
        this.authenticationStateProvider = authenticationStateProvider;
    }

    public bool IsLoggedIn => !string.IsNullOrEmpty(GetUserToken());

    public string GetUserToken() => syncLocalStorageService.GetToken();

    public string GetEmail() => syncLocalStorageService.GetEmail();
    public string GetUserName() => syncLocalStorageService.GetToken();
    public string GetUserRole() => syncLocalStorageService.GetRole();
    public Guid GetUserId() => syncLocalStorageService.GetUserId();

    public async Task<bool> Login(LoginUserCommand command)
    {
        var httpResponse = await httpClient.PostAsJsonAsync("api/User/Login", command);

        if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var responseStr = await httpResponse.Content.ReadAsStringAsync();
                var validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr);
                throw new DataBaseValidationException(validation.FlattenErrors);
            }

            return false;
        }

        var response = JsonSerializer.Deserialize<LoginUserViewModel>(
            await httpResponse.Content.ReadAsStringAsync()
        );

        if (!string.IsNullOrEmpty(response?.Token))
        {
            syncLocalStorageService.SetToken(response.Token);
            syncLocalStorageService.SetUsername(response.LastName);
            syncLocalStorageService.SetUserId(response.Id);
            syncLocalStorageService.SetRole(response.Role);
            syncLocalStorageService.SetEmail(response.Email);

            ((AuthStateProvider)authenticationStateProvider).NotifyUserLogin(response.LastName, response.Id);

            // artık header elle eklenmeyecek, AuthTokenHandler halledecek
            return true;
        }

        return false;
    }

    public void Logout()
    {
        syncLocalStorageService.RemoveItem(LocalStorageExtension.TokenName);
        syncLocalStorageService.RemoveItem(LocalStorageExtension.UserName);
        syncLocalStorageService.RemoveItem(LocalStorageExtension.UserId);
        syncLocalStorageService.RemoveItem(LocalStorageExtension.Role);
        syncLocalStorageService.RemoveItem(LocalStorageExtension.Email);

        ((AuthStateProvider)authenticationStateProvider).NotifyUserLogout();
    }
}