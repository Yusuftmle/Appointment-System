
using Application.Models;
using Application.Queries.ResetPassword;
using Application.Queries.user;
using Application.RequestModels.User.CreateUser;
using Application.RequestModels.User.PasswordComment;
using Application.RequestModels.User.UpdateUser;
using Application.Results;
using HotelVR.Common.Infrastructure.Exceptions;
using HotelVR.WebApp.Infrastructure.Services.Interfaceses;
using System.Net.Http;
using System.Net.Http.Json;

namespace HotelVR.WebApp.Infrastructure.Services
{
    public class UserService : IUserService
    {

        private readonly HttpClient _httpClient;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        public async Task<Guid> CreateUserAsync(CreateUserCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync("api/User/CreateUser", command);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                throw new DataBaseValidationException($"API Hatası: {response.StatusCode} - {errorMessage}");
            }

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task<List<UserDto>> GetAllUserList()
        {
            var response = await _httpClient.GetAsync("api/User/GetAllUser");

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                throw new DataBaseValidationException($"API Hatası: {response.StatusCode} - {errorMessage}");
            }

            return await response.Content.ReadFromJsonAsync<List<UserDto>>();
        }

        // Profil Bilgilerini Getir
        public async Task<UserDto?> GetMyProfileAsync()
        {
            var response = await _httpClient.GetAsync("api/user/UserDetail");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserDto>();
            }

            return null;
        }

        // Profil Güncelleme
        public async Task<bool> UpdateProfileAsync(UpdateUserCommand updateUserCommand)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/User-Update", updateUserCommand);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> ChangePasswordAsync(ChangePasswordCommand model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/ChangePassword", model);

            if (response.IsSuccessStatusCode)
                return true;

            var errorContent = await response.Content.ReadAsStringAsync();
            // Burada istersen loglayabilir veya kullanıcıya gösterebilirsin.
            return false;
        }

        public async Task<Result> RequestPasswordResetAsync(RequestPasswordResetCommand resetCommand)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/request-password-reset",resetCommand);
            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result!;
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/reset-password",command);
            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result!;
        }

       
    }
}
