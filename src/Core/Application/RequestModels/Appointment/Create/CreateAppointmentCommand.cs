using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.RequestModels.Appointment
{
    public class CreateAppointmentCommand:IRequest<Guid>
    {
       
        public Guid? UserId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public AppointmentType Type { get; set; } = AppointmentType.FaceToFace;

        public bool IsConfirmed { get; set; }=true;
    }
}
