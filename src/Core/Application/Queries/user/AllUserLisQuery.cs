using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Queries.user
{
    public class AllUserListQuery:IRequest<List<UserDto>>
    {
       
        public string Email { get; set; } // Kullanıcı e-posta adresi
        public Guid Id { get; set; } // Kullanıcı kimliği
        public string firstName { get; set; } // Kullanıcı adı
        public string lastName { get; set; } // Kullanıcı soyadı
        public string? Role { get; set; } // Kullanıcı rolü: Customer, Manager, Admin
    }
}
