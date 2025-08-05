using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class TimeSlotDto
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }      
         public DateTime StartTime { get; set; }
     
        public bool IsBooked { get; set; }
    }
}
