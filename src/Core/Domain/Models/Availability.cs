using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Availability:BaseEntity
    {
        public DateTime AppointmentDateTime { get; set; }
        public bool IsBooked { get; set; }
    }
}
