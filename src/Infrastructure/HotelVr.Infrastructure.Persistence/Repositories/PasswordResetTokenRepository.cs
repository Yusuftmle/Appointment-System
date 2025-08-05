using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Context;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;

namespace HotelRv.Infrastructure.Persistence.Repositories
{
    public class PasswordResetTokenRepository:GenericRepository<PasswordResetToken>, IPasswordResetTokenRepository
    {
        public PasswordResetTokenRepository(HotelVRContext dbContext) : base(dbContext, dbContext.Set<PasswordResetToken>())
        {

        }
        public async Task<PasswordResetToken?> GetByTokenAsync(string token)
        {
            return await FirstOrDefaultAsync(x => x.Token == token && x.ExpirationDate > DateTime.UtcNow);
        }

        public async Task<PasswordResetToken?> GetByEmailAsync(string email)
        {
            return await FirstOrDefaultAsync(x => x.UserEmail == email);
        }

        public async Task DeleteExpiredTokensAsync()
        {
            // GenericRepository'deki BulkDelete metodunu kullan
            await BulkDelete(x => x.ExpirationDate <= DateTime.UtcNow);
        }

    }
}
