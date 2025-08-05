using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MediatR;

namespace Application.RequestModels.User.UpdateUser
{
    public class UpdateUserCommand:IRequest<Guid>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; } // Kullanıcı telefon numarası

        public string FullName => $"{FirstName} {LastName}";

    }
}
