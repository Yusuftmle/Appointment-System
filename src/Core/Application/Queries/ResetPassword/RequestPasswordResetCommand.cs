using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Results;
using MediatR;

namespace Application.Queries.ResetPassword
{
    public class RequestPasswordResetCommand:IRequest<IResult>
    {
        public string Email { get; set; }
    }
}
