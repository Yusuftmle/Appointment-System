using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Context;

namespace HotelRvDbContext.Infrastructure.Persistence.Repositories
{
    public class BlogPostRepository:GenericRepository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(HotelVRContext dbContext) : base(dbContext, dbContext.Set<BlogPost>())
        {
  
        }
       

    }
   
}
