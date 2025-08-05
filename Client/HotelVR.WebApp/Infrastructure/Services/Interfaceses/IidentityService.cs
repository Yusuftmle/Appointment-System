using Application.RequestModels.User.LoginCommand;

namespace HotelVR.WebApp.Infrastructure.Services.Interfaceses
{
    public interface IidentityService
    {
        bool IsLoggedIn { get; }

        Guid GetUserId();
        string GetUserName();
        string GetUserToken();
        string GetEmail();
        Task<bool> Login(LoginUserCommand command);
        void Logout();
    }
}