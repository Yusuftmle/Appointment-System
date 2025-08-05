using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;

namespace HotelRv.Infrastructure.Persistence.Repositories
{
    public interface IPasswordResetTokenRepository:IGenericRepository<PasswordResetToken>
    {
        Task<PasswordResetToken?> GetByTokenAsync(string token);
        Task<PasswordResetToken?> GetByEmailAsync(string email);
        Task DeleteExpiredTokensAsync();
    }

}