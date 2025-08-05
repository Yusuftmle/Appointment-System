using Application.Models;
using Application.RequestModels.BlogTag.Create;
using AutoMapper;
using Domain.Models;
using HotelRv.Infrastructure.Persistence.Repositories;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class CreateBlogTagCommandHandler : IRequestHandler<CreateBlogTagCommand, ResultModel<BlogTagDto>>
{
    private readonly IBlogTagRepository _blogTagRepository;
    private readonly IBlogPostTagRepository _blogPostTagRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateBlogTagCommandHandler> _logger;

    public CreateBlogTagCommandHandler(
        IBlogTagRepository blogTagRepository,
        IBlogPostTagRepository blogPostTagRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateBlogTagCommandHandler> logger)
    {
        _blogTagRepository = blogTagRepository;
        _blogPostTagRepository = blogPostTagRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResultModel<BlogTagDto>> Handle(CreateBlogTagCommand request, CancellationToken cancellationToken)
    {
         await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Validasyonlar
            if (string.IsNullOrWhiteSpace(request.Name))
                return ResultModel<BlogTagDto>.Failure("Etiket ismi boş olamaz");

            if (request.Name.Length > 50)
                return ResultModel<BlogTagDto>.Failure("Etiket ismi 50 karakterden uzun olamaz");

            // Case-insensitive kontrol
            var normalizedName = request.Name.Trim().ToLower();
            var existing = await _blogTagRepository.GetSingleAsync(
                x => EF.Functions.Collate(x.Name, "SQL_Latin1_General_CP1_CI_AS") == normalizedName
                );

            if (existing != null)
                return ResultModel<BlogTagDto>.Failure("Bu isimde bir blog etiketi zaten var");

            // Yeni tag oluştur
            var blogTag = new BlogTag
            {
                Name = request.Name.Trim(),
                CreateDate = DateTime.UtcNow
            };

            await _blogTagRepository.AddAsync(blogTag);

            // İsteğe bağlı: Posta ata
            if (request.AssignToPostId.HasValue)
            {
                await _blogPostTagRepository.AddAsync(new BlogPostTag
                {
                    BlogPostId = request.AssignToPostId.Value,
                    BlogTagId = blogTag.Id,
                    CreateDate = DateTime.UtcNow
                });
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("Yeni blog etiketi oluşturuldu: {TagName} (ID: {TagId})",
                blogTag.Name, blogTag.Id);

            return ResultModel<BlogTagDto>.Success(_mapper.Map<BlogTagDto>(blogTag));
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            _logger.LogError(ex, "Blog etiketi oluşturulurken hata oluştu: {Message}", ex.Message);

            return ResultModel<BlogTagDto>.Failure("Blog etiketi oluşturulurken beklenmeyen bir hata oluştu");
        }
    }
}