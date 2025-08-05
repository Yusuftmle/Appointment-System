using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.RequestModels.TimeSlotCommand.Create
{
    public class AddAvailableTimeSlotCommand : IRequest<bool>
    {
        public DateTime AppointmentDateTime { get; set; }
        public bool IsBooked { get; set; }

    }
}
