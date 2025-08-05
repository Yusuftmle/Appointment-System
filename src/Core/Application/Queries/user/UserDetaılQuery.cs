 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Queries.user
{
    public class UserDetaılQuery:IRequest<UserDto>
    {
        public UserDetaılQuery(Guid ıd)
        {
            Id = ıd;
        }

        public Guid Id { get; set; }
    }
}
