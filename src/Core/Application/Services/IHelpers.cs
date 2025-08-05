using Application.Repositories.Interfaces;

namespace Application.Services
{
    public interface IHelpers
    {
        /// <summary>
        /// Verilen metinden SEO uyumlu bir slug üretir.
        /// </summary>
        /// <param name="text">Slug üretilecek başlık metni</param>
        /// <returns>SEO uyumlu slug stringi</returns>
        string GenerateSlug(string text);
       
    }
}