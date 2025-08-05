using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Queries.ResetPassword
{
    public class ResetPasswordCommand:IRequest<IResult>
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
