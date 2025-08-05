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
    public class ServiceRepository : GenericRepository<Service>,IServiceRepository
    {
        public ServiceRepository(HotelVRContext dbContext) : base(dbContext, dbContext.Set<Service>())
        {
        }
    }
}
