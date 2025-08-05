using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;

namespace Application.Repositories.Interfaces
{
    public interface IBlogPostRepository : IGenericRepository<BlogPost>
    {
        
    }
}
