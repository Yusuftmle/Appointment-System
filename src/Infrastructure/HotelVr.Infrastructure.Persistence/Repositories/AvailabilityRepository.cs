using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelRvDbContext.Infrastructure.Persistence.Repositories
{
    public class AvailabilityRepository:GenericRepository<Availability>, IAvailabilityRepository
    {

        public AvailabilityRepository(HotelVRContext dbContext) : base(dbContext, dbContext.Set<Availability>())
        {
           
        }

       
    }
}
