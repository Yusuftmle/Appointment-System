using Microsoft.AspNetCore.Mvc;

namespace HotelApi.Infrastructure
{
    public class FileUploadRequest
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }
    }
}
