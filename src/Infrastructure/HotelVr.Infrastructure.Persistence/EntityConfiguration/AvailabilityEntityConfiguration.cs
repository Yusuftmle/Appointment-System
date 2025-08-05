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
    public class AvailabilityEntityConfiguration:BaseEntityConfiguration<Availability>
    {
        public void Configure(EntityTypeBuilder<Availability> builder)
        {
            base.Configure(builder);
            builder.ToTable("Availability", HotelVRContext.DEFAULT_SCHEMA);

            builder.Property(av => av.AppointmentDateTime).IsRequired();
           


           
        }
    }
}
