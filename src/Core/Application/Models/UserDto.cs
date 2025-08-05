using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Models
{
    public  class UserDto
    {
        public string Email { get; set; } // Kullanıcı e-posta adresi
        public Guid Id { get; set; }
        public string firstName { get; set; } // Kullanıcı adı
        public string lastName { get; set; } // Kullanıcı soyadı

        public string FullName { get;set; }
        public string PhoneNumber { get; set; } // Kullanıcı telefon numarası



        public string? Role { get; set; } // Kullanıcı rolü: Customer, Manager, Admin
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
        // Navigasyon Özellikleri
        public List<Appointment> Appointment { get; set; } 
    }
}
