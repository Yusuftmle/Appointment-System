using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Page;
using MediatR;

namespace Application.Queries.Appointments
{
    public class GetPaginatedAppointmentsQuery:BasePagedQuery,IRequest<PagedViewModel<AppointmentDto>>
    {
        public Guid? UserId { get; set; }

        public GetPaginatedAppointmentsQuery(Guid? userId, int page, int pageSize) : base(page, pageSize)
        {
            UserId = userId; 
        }

    }
}
