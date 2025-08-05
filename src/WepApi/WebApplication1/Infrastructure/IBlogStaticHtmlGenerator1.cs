using Application.Models;
using Application.RequestModels.BlogPost.CreateBlog;

namespace HotelApi.Infrastructure
{
    public interface IBlogStaticHtmlGenerator
    {
        Task GenerateAsync(CreateBlogPostCommand blog);
    }
}