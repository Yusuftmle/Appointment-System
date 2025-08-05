using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.RequestModels.Appointment.Delete
{
    public class DeleteAppointmentCommand:IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}
