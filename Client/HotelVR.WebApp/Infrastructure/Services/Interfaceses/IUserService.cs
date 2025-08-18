
using Application.Models;
using Application.Queries.ResetPassword;
using Application.RequestModels.User.CreateUser;
using Application.RequestModels.User.PasswordComment;
using Application.RequestModels.User.UpdateUser;
using Application.Results;

namespace HotelVR.WebApp.Infrastructure.Services.Interfaceses
{
    public interface IUserService
    {
        Task<Guid> CreateUserAsync(CreateUserCommand command);
        Task<List<UserDto>> GetAllUserList();
        Task<UserDto?> GetMyProfileAsync();
        Task<bool> UpdateProfileAsync(UpdateUserCommand updateUserCommand);
        Task<bool> ChangePasswordAsync(ChangePasswordCommand model);
        Task<Result> ResetPasswordAsync(ResetPasswordCommand command);
        Task<Result> RequestPasswordResetAsync(RequestPasswordResetCommand resetCommand);
    }
}