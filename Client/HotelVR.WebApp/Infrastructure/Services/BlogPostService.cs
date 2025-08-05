using Application.Models;
using System.Net.Http.Json;
using HotelVR.WebApp.Infrastructure.Services.Interfaceses;
using Application.Models.Page;
using Application.RequestModels.BlogPost.CreateBlog;
using Application.RequestModels.BlogPost.UpdateBlog;
using Application.RequestModels.BlogTag.Create;
using Application.RequestModels.BlogTag.Delete;
using Application.Queries.Blog;
using Application.RequestModels.BlogTag.Update;

namespace HotelVR.WebApp.Infrastructure.Services
{
    public class BlogPostService: IBlogPostService
    {
        private readonly HttpClient _http;

        public BlogPostService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("API");
        }

        public async Task<List<BlogPostDto>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            var response = await _http.GetFromJsonAsync<PagedViewModel<BlogPostDto>>($"api/Blog/MainPageBlog?page={page}&pageSize={pageSize}");
            return response?.Results?.ToList() ?? new List<BlogPostDto>();

        }


        public async Task<BlogPostDto> GetBySlugAsync(string slug)
        {
            return await _http.GetFromJsonAsync<BlogPostDto>($"api/Blog/{slug}");
        }

        public async Task<Guid> CreateAsync(CreateBlogPostCommand dto)
        {
            var response = await _http.PostAsJsonAsync("api/Blog/Create-Blog", dto);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task UpdateAsync(UpdateBlogPostComamnd dto)
        {
            var response = await _http.PostAsJsonAsync("api/Blog/update-blogpost", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateBlogTag(CreateBlogTagCommand command)
        {
            var response = await _http.PostAsJsonAsync("api/Blog/Create-BlogTag", command);
            response.EnsureSuccessStatusCode();

        }
        public async Task DeleteBlogTag(Guid id)
        {
            var response = await _http.DeleteAsync($"api/Blog/delete-Tag/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteBlog(Guid id)
        {
            var response = await _http.DeleteAsync($"api/Blog/delete-Blog/{id}");
            response.EnsureSuccessStatusCode();
        }
         
        public async Task<List<BlogTagDto>> GetAllBlogTag()
        {
            var response = await _http.GetFromJsonAsync<List<BlogTagDto>>($"api/Blog/GetAllBlogTag");
            return response?.ToList() ?? new List<BlogTagDto>();
        }

        public async Task<List<BlogPostListItemDto>> GetLastPostAsync(int page = 1, int pageSize = 5)
        {
            var response = await _http.GetFromJsonAsync<PagedViewModel<BlogPostListItemDto>>($"api/Blog/GetLastPost?page={page}&pageSize={pageSize}");
            return response?.Results?.ToList() ?? new List<BlogPostListItemDto>();

        }

        public async Task UpdateBlogTagAsync(UpdateBlogTagCommand dto)
        {
            var response = await _http.PostAsJsonAsync("api/Blog/Update-BlogTag", dto);
            response.EnsureSuccessStatusCode();
        }
    }
}
