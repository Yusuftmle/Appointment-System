using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Queries.Appointments
{
    public class GetByIdAppointmentsQuery:IRequest<AppointmentDto>
    {
        public GetByIdAppointmentsQuery(Guid Id)
        {
           this.Id = Id;
        }

        public Guid Id { get; set; }
    }
}
