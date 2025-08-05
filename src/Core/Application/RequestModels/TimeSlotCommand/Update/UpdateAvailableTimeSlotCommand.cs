using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.RequestModels.TimeSlotCommand.Update
{
    public class UpdateAvailableTimeSlotCommand:IRequest<Guid>
    {
       public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime AppointmentDateTime { get; set; }    
    }
}
