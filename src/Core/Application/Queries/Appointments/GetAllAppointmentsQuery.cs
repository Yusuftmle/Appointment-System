using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Queries.Appointments
{
    public class GetAllAppointmentsQuery:IRequest<List<AppointmentDto>>
    {
       
    }
}
