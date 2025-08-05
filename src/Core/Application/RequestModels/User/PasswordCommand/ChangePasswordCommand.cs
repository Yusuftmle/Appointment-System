using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;

namespace Application.RequestModels.User.PasswordComment
{
    public class ChangePasswordCommand:IRequest<bool>
    {
        public ChangePasswordCommand()
        {
            
        }
        public ChangePasswordCommand(Guid? Id, string oldPassword, string newPassword)
        {
            
            this.oldPassword = oldPassword;
            this.newPassword = newPassword;
        }
        [JsonIgnore]
        public Guid? UserId { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }

       
    }
}
