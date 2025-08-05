using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelRvDbContext.Infrastructure.Persistence.EntityConfiguration
{
    public class AppointmentEntityConfiguration : BaseEntityConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            base.Configure(builder);
            builder.ToTable("Appointment", HotelVRContext.DEFAULT_SCHEMA);

            // User ilişkisi - Cascade silme davranışı
            builder.HasOne(a => a.User)
                   .WithMany(u => u.Appointments)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse randevular da silinsin

            // Service ilişkisi - Cascade silme
            builder.HasOne(a => a.Service)
                   .WithMany(s => s.Appointments)
                   .HasForeignKey(a => a.ServiceId)
                   .OnDelete(DeleteBehavior.Cascade); // Servis silinirse randevular da silinsin

          
        }
    }
}
