using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.RequestModels.Service.Delete
{
    public class DeleteServiceCommand :IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}
