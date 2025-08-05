using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;

namespace HotelRv.Infrastructure.Persistence.Repositories
{
    public interface IBlogPostTagRepository:IGenericRepository<BlogPostTag>
    {
    }
}