using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using AutoMapper;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using Application.Services;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;
using Microsoft.IdentityModel.Logging;
using HotelRv.Infrastructure.Persistence.Repositories;

namespace Application.RequestModels.BlogPost.CreateBlog
{
    public class CreateBlogPostCommandHandler : IRequestHandler<CreateBlogPostCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHelpers helpers;
        private readonly IBlogTagRepository blogTagRepository;
        private readonly IBlogPostTagRepository blogPostTagRepository;
       


        public CreateBlogPostCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IBlogPostRepository blogPostRepository, IHelpers helpers, IBlogTagRepository blogTagRepository, IBlogPostTagRepository blogPostTagRepository)
        {
            this.mapper = mapper;
            _unitOfWork = unitOfWork;
            _blogPostRepository = blogPostRepository;
            this.helpers = helpers;
            this.blogTagRepository = blogTagRepository;
            this.blogPostTagRepository = blogPostTagRepository;
        }

        public async Task<Guid> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Tag kontrolü
                var existTag = await blogTagRepository.GetSingleAsync(x => x.Id == request.BlogTagId);
                if (existTag == null)
                    throw new DataBaseValidationException("Blog Tag bulunamadı");

                if (string.IsNullOrWhiteSpace(request.Title))
                    throw new DataBaseValidationException("Başlık boş olamaz");

                if (string.IsNullOrWhiteSpace(request.Content))
                    throw new DataBaseValidationException("İçerik boş olamaz");

                // Slug'ı mapping'den ÖNCE set et
                var slug = helpers.GenerateSlug(request.Title);
                request.Slug = slug; // ÖNEMLİ: Request'e slug'ı ata

                // Slug benzersiz mi?
                var existingSlug = await _blogPostRepository.GetSingleAsync(x => x.Slug == slug);
                if (existingSlug != null)
                    throw new DataBaseValidationException("Bu slug zaten kullanılıyor");

                // BlogPost oluştur (artık slug request'te var)
                var blogPost = mapper.Map<Domain.Models.BlogPost>(request);

                await _blogPostRepository.AddAsync(blogPost);

                // BlogPostTag ilişkisini kur
                var blogPostTag = new BlogPostTag
                {
                    BlogPostId = blogPost.Id,
                    BlogTagId = existTag.Id
                };
                await blogPostTagRepository.AddAsync(blogPostTag);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return blogPost.Id;
            }
            catch (Exception ex)
            {
                throw new DataBaseValidationException("Blog yazısı oluşturulurken bir hata oluştu", ex);
            }
        }

    }
}
