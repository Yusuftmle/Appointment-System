using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Queries.user;
using MediatR;

namespace Application.RequestModels.User.LoginCommand
{
    public class LoginUserCommand : IRequest<LoginUserViewModel>
    {
        
        public string? Email { get; set; }
        
        public string? PasswordHash { get; set; }

        public LoginUserCommand()
        {
            
        }

        public LoginUserCommand(string? email, string? password)
        {
            Email = email;
            PasswordHash = password;



        }





    }
}
