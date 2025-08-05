using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Models
{
    public class Service:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Appointment> Appointments { get; set; } = new();
        public AppointmentType AllowedType { get; set; } = AppointmentType.FaceToFace;

    }
}
