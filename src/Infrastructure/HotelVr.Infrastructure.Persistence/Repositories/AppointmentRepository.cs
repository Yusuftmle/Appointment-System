using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelRvDbContext.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository:GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HotelVRContext dbContext) : base(dbContext, dbContext.Set<Appointment>())
        {
          
        }

    }
}
