using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using AutoMapper;
using HotelVR.Common.Infrastructure;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.User.PasswordComment
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public ChangePasswordCommandHandler(IMapper mapper, IUserRepository userRepository)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            // Eğer UserId komut içinde yoksa hata ver ya da dışardan parametre olarak alınabilir
            if (!request.UserId.HasValue)
                throw new ArgumentNullException(nameof(request.UserId), "UserId boş olamaz!");

            var dbUser = await userRepository.GetByIdAsync(request.UserId.Value);
            if (dbUser == null)
                throw new DataBaseValidationException("Kullanıcı bulunamadı!");


            //  Şifreler boş mu kontrol et
            if (string.IsNullOrWhiteSpace(request.oldPassword) || string.IsNullOrWhiteSpace(request.newPassword))
                throw new DataBaseValidationException("Şifreler boş olamaz!");


            var hashingRequestPassword = PasswordEncryptor.Encrpt(request.oldPassword);
            //  Eski şifreyi doğrula
            if (dbUser.PasswordHash != hashingRequestPassword)
                throw new DataBaseValidationException("Eski şifre yanlış!");

            // 5️⃣ Yeni şifreyi güncelle
            var hashingNewPassword = PasswordEncryptor.Encrpt(request.newPassword);
            dbUser.PasswordHash = hashingNewPassword;
            await userRepository.UpdateAsync(dbUser);

            return true;

        }
    }
}
