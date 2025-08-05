using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using Application.Results;
using HotelRv.Infrastructure.Persistence.Repositories;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure;
using MediatR;

namespace Application.Queries.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResult>
    {
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly IUserRepository _userService;
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordCommandHandler(
            IPasswordResetTokenRepository tokenRepository,
            IUserRepository userService,
            IUnitOfWork unitOfWork)
        {
            _tokenRepository = tokenRepository;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Validate token
                var resetToken = await _tokenRepository.GetByTokenAsync(request.Token);
                if (resetToken == null)
                {
                    return Result.Failure("Invalid or expired reset token.");
                }

                // 2. Get user
                var user = await _userService.FirstOrDefaultAsync(u => u.Email == resetToken.UserEmail);
                if (user == null)
                {
                    return Result.Failure("User not found.");
                }

                // 3. Hash new password
                var hashedPassword = PasswordEncryptor.Encrpt(request.NewPassword);

                // 4. Update user password
                user.PasswordHash = hashedPassword;
                await _userService.UpdateAsync(user);

                // 5. Delete used token
                await _tokenRepository.DeleteAsync(resetToken.Id);

                // 6. Commit transaction
                await _unitOfWork.SaveChangesAsync();

                return Result.Success("Password has been reset successfully.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occurred: {ex.Message}");
            }

        }
    }
}
