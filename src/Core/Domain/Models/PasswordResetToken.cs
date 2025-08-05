using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PasswordResetToken:BaseEntity
    {
        public string UserEmail { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public PasswordResetToken(string userEmail, string token, DateTime expirationDate)
        {
            UserEmail = userEmail;
            Token = token;
            ExpirationDate = expirationDate;
        }

        public PasswordResetToken() { } // Parameterless constructor for EF Core
    }
}
