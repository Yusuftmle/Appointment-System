using Microsoft.AspNetCore.Components.Forms;

namespace HotelVR.WebApp.Infrastructure.Services
{
    public class UploadService
    {
        private readonly HttpClient _http;

        public UploadService(HttpClient http) => _http = http;

        public async Task<string> UploadImage(IBrowserFile file)
        {
            var content = new MultipartFormDataContent();
            var stream = file.OpenReadStream(file.Size);
            content.Add(new StreamContent(stream), "file", file.Name);

            var response = await _http.PostAsync("/api/upload", content);
            return await response.Content.ReadAsStringAsync(); // URL dönecek
        }
    }
}
