using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Context;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;

namespace HotelRv.Infrastructure.Persistence.Repositories
{
    public class BlogTagRepository : GenericRepository<BlogTag>, IBlogTagRepository
    {
        public BlogTagRepository(HotelVRContext dbContext) : base(dbContext, dbContext.Set<BlogTag>())
        {

        }
       
    }
}
