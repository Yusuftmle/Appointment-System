using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using Application.Results;
using Application.Services;
using Domain.Models;
using HotelRv.Infrastructure.Persistence.Repositories;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.Queries.ResetPassword
{
    public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, IResult>
    {
        private readonly IPasswordResetTokenRepository _tokenRepository;
        private readonly IUserRepository _userService;
        private readonly ImailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenerateSecureToken _generateSecureToken;

        public RequestPasswordResetCommandHandler(
            IPasswordResetTokenRepository tokenRepository,
            IUserRepository userService,
            ImailService emailService,
            IUnitOfWork unitOfWork,
            IGenerateSecureToken generateSecureToken)
        {
            _tokenRepository = tokenRepository;
            _userService = userService;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _generateSecureToken = generateSecureToken;
        }

        public async Task<IResult> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Check if user exists
                var userExists = await _userService.FirstOrDefaultAsync(i=>i.Email== request.Email);
                if (userExists == null)
                {
                    return Result.Success("If the email exists, a password reset link has been sent.");
                }


                // 2. Delete existing token for this email
                var existingToken = await _tokenRepository.GetByEmailAsync(request.Email);
                if (existingToken != null)
                {
                    await _tokenRepository.DeleteAsync(existingToken.Id);
                }

                // 3. Generate new reset token
                var resetToken = _generateSecureToken.generateSecureToken();
                var expirationDate = DateTime.UtcNow.AddHours(1); // Token expires in 1 hour

                // 4. Save token to database
                var passwordResetToken = new PasswordResetToken(request.Email, resetToken, expirationDate);
                await _tokenRepository.AddAsync(passwordResetToken);
                // UnitOfWork kullandığın için burada SaveChanges çağırmamaliyiz
                // await _unitOfWork.SaveChangesAsync();

                // 5. Send email
                await _emailService.SendPasswordResetEmailAsync(request.Email, resetToken);

                return Result.Success("If the email exists, a password reset link has been sent.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occurred: {ex.Message}");
            }
        }

      
    }
}
