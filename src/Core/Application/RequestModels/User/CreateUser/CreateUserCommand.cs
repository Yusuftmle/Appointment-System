using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using MediatR;
using static Application.Enums.Role;

namespace Application.RequestModels.User.CreateUser
{
    public class CreateUserCommand:IRequest<Guid>
    {
        public string FullName => $"{firstName} {lastName}"; // Otomatik oluşturuluyor!
        public string Email { get; set; } // Kullanıcı e-posta adresi

        public string firstName { get; set; } // Kullanıcı adı
        public string lastName { get; set; } // Kullanıcı soyadı
        public string PasswordHash { get; set; } // Şifre hash
        public string PhoneNumber { get; set; } // Telefon numarası

        public Roles? Role { get; set; } = Roles.User; // Kullanıcının rolü

    }
}
