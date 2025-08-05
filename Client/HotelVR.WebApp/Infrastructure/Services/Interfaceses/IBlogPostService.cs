using Application.Models;
using Application.Queries.Blog;
using Application.RequestModels.BlogPost.CreateBlog;
using Application.RequestModels.BlogPost.UpdateBlog;
using Application.RequestModels.BlogTag.Create;
using Application.RequestModels.BlogTag.Update;

namespace HotelVR.WebApp.Infrastructure.Services.Interfaceses
{
    public interface IBlogPostService
    {
        Task<List<BlogPostDto>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<List<BlogPostListItemDto>> GetLastPostAsync(int page = 1, int pageSize = 5);
        Task<BlogPostDto> GetBySlugAsync(string slug);
        Task CreateBlogTag(CreateBlogTagCommand command);
        Task UpdateBlogTagAsync(UpdateBlogTagCommand dto);
        Task<Guid> CreateAsync(CreateBlogPostCommand dto);
        Task UpdateAsync(UpdateBlogPostComamnd dto);
        Task DeleteBlog(Guid id);
        Task DeleteBlogTag(Guid id);
        Task<List<BlogTagDto>> GetAllBlogTag();
    }
}