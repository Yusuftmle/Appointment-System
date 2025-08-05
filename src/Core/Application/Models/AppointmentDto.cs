using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Models
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public Guid ServiceId { get; set; }
        public string? TimeSlot { get; set; }
        public string?  ServiceName { get; set; }    
        public DateTime AppointmentDate { get; set; }
        public AppointmentType appointmentType { get; set; } = AppointmentType.FaceToFace;
        public ReservationStatus ReservationStatus { get; set; }
        public bool IsConfirmed { get; set; }
        public string PhoneNumber { get; set; }
    }
}
