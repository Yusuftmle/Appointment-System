using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.user
{
    public class CreateUserViewModel
    {
        public string FullName { get; set; } // Kullanıcı tam adı
        public string Email { get; set; } // Kullanıcı e-posta adresi
        public string ConfirmPassword { get; set; }
        public string firstName { get; set; } // Kullanıcı adı
        public string lastName { get; set; } // Kullanıcı soyadı
        public string PasswordHash { get; set; } // Şifre hash
        public string PhoneNumber { get; set; } // Telefon numarası
    }
}
