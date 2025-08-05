using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Repositories.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Models;
using HotelRv.Infrastructure.Persistence.Repositories;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.RequestModels.BlogPost.UpdateBlog
{
    public class UpdateBlogPostCommandHandler : IRequestHandler<UpdateBlogPostComamnd, Guid>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IBlogPostTagRepository _blogPostTagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelpers _helpers;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateBlogPostCommandHandler> _logger;

        public UpdateBlogPostCommandHandler(
            IBlogPostRepository blogPostRepository,
            IBlogTagRepository blogTagRepository,
            IBlogPostTagRepository blogPostTagRepository,
            IUnitOfWork unitOfWork,
            IHelpers helpers,
            IMapper mapper,
            ILogger<UpdateBlogPostCommandHandler> logger)
        {
            _blogPostRepository = blogPostRepository;
            _blogTagRepository = blogTagRepository;
            _blogPostTagRepository = blogPostTagRepository;
            _unitOfWork = unitOfWork;
            _helpers = helpers;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(UpdateBlogPostComamnd request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Blog post güncelleme başladı - ID: {BlogPostId}", request.Id);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _logger.LogDebug("Transaction başlatıldı");

                // 1. Blog post kontrolü
                var blogPost = await _blogPostRepository.GetByIdAsync(request.Id);
                if (blogPost == null)
                {
                    _logger.LogWarning("Blog post bulunamadı - ID: {BlogPostId}", request.Id);
                    throw new DataBaseValidationException("Blog post bulunamadı");
                }

                _logger.LogDebug("Blog post bulundu: {Title}", blogPost.Title);

                // 2. Tag kontrolü
                var tag = await _blogTagRepository.GetByIdAsync(request.BlogTagId);
                if (tag == null)
                {
                    _logger.LogWarning("Blog tag bulunamadı - ID: {TagId}", request.BlogTagId);
                    throw new DataBaseValidationException("Blog tag bulunamadı");
                }

                _logger.LogDebug("Blog tag bulundu: {TagName}", tag.Name);

                // 3. Blog post bilgilerini güncelle
                _logger.LogDebug("Blog post güncelleniyor");

                blogPost.Title = request.Title;
                blogPost.Content = request.Content;
                blogPost.IsPublished = request.IsActive;
                blogPost.Summary = request.Summary;
                blogPost.CoverImageUrl = request.CoverImageUrl;
                blogPost.Keywords = request.Keywords;
                blogPost.Slug = _helpers.GenerateSlug(request.Title);
                blogPost.UpdateDate = DateTime.UtcNow;

                await _blogPostRepository.UpdateAsync(blogPost);
                _logger.LogDebug("Blog post güncellendi");

                // 4. Mevcut tag ilişkilerini sil
                _logger.LogDebug("Mevcut tag ilişkileri siliniyor");
                var existingTags = await _blogPostTagRepository.GetListByPostIdAsync(blogPost.Id);

                foreach (var existingTag in existingTags)
                {
                    await _blogPostTagRepository.DeleteAsync(existingTag);
                }

                _logger.LogDebug("Silinen tag ilişki sayısı: {Count}", existingTags.Count);

                // 5. Yeni tag ilişkisini ekle
                _logger.LogDebug("Yeni tag ilişkisi ekleniyor");
                var newBlogPostTag = new BlogPostTag
                {
                    BlogPostId = blogPost.Id,
                    BlogTagId = request.BlogTagId
                };

                await _blogPostTagRepository.AddAsync(newBlogPostTag);
                _logger.LogDebug("Yeni tag ilişkisi eklendi");

                // 6. Değişiklikleri kaydet
                await _unitOfWork.SaveChangesAsync();
                _logger.LogDebug("Değişiklikler kaydedildi");

                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Blog post başarıyla güncellendi - ID: {BlogPostId}", blogPost.Id);

                return blogPost.Id;
            }
            catch (DataBaseValidationException ex)
            {
                _logger.LogError(ex, "Validasyon hatası: {Message}", ex.Message);
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Blog güncelleme hatası - BlogPost ID: {BlogPostId}", request.Id);
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Blog güncelleme işlemi başarısız: {ex.Message}", ex);
            }
        }

    }
}
