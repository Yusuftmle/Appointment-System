using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Queries.user
{
    public class ForgotPasswordComand:IRequest<bool>
    {
        public string Email { get; set; }
    }
}
