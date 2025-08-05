
namespace Application.Services
{
    public interface ImailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetToken);
    }
}