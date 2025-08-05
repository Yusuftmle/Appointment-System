using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Context;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;

namespace HotelRv.Infrastructure.Persistence.Repositories
{
    public class BlogPostTagRepository : GenericRepository<BlogPostTag>, IBlogPostTagRepository
    {
        public BlogPostTagRepository(HotelVRContext dbContext) : base(dbContext, dbContext.Set<BlogPostTag>())
        {

        }


    }
}
