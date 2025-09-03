using Application.Models;
using Application.Queries.Blog;
using HotelRv.Infrastructure.Persistence.Repositories;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetBlogPostTagIdQueryHandler : IRequestHandler<GetBlogPostTagIdQuery, BlogTagDto>
{
    private readonly IBlogPostTagRepository blogPostTagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetBlogPostTagIdQueryHandler(IBlogPostTagRepository blogPostTagRepository, IUnitOfWork unitOfWork)
    {
        this.blogPostTagRepository = blogPostTagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BlogTagDto> Handle(GetBlogPostTagIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = blogPostTagRepository.AsQueryable()
                .Where(i => i.BlogTagId == request.Id && !i.IsDeleted);

            var result = await query.Select(i => new BlogTagDto
            {
                Id = i.BlogTag.Id, // BlogTag Id'sini alıyoruz
                Name = i.BlogTag.Name, // BlogTag Name'i alıyoruz
            }).FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                throw new DataBaseValidationException("BlogTag bulunamadı");

            await _unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch (Exception ex)
        {

            await _unitOfWork.RollbackTransactionAsync();
            throw new DataBaseValidationException($"Hata oluştu: {ex.Message}");
        }
    }
}
