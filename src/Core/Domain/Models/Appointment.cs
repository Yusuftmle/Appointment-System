using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Enums;
using Domain.Enums;

namespace Domain.Models
{
    public class Appointment:BaseEntity
    {
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        public AppointmentType Type { get; set; } = AppointmentType.FaceToFace;


    }
}
