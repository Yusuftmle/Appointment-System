using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelRv.Infrastructure.Persistence.Repositories;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.BlogTag.Update
{
    public class UpdateBlogTagCommandHandler : IRequestHandler<UpdateBlogTagCommand, Guid>
    {
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IBlogPostTagRepository _blogPostTagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBlogTagCommandHandler(
            IBlogTagRepository blogTagRepository,
            IBlogPostTagRepository blogPostTagRepository,
            IUnitOfWork unitOfWork)
        {
            _blogTagRepository = blogTagRepository;
            _blogPostTagRepository = blogPostTagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpdateBlogTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var tag = await _blogTagRepository.GetByIdAsync(request.Id);
                if (tag == null)
                    throw new DataBaseValidationException("Blog tag bulunamadı");

                // PROBLEM: Aynı isimde başka tag var mı kontrol et
                var existingTagWithSameName = await _blogTagRepository.GetSingleAsync(x=>x.Name==request.Name);
                if (existingTagWithSameName != null && existingTagWithSameName.Id != request.Id)
                {
                    throw new DataBaseValidationException("Bu isimde bir kategori zaten mevcut");
                }

                tag.Name = request.Name;
              

                await _blogTagRepository.UpdateAsync(tag);

                // PROBLEM: Bu satır yanlış - BlogPostTag repository'den tag ID ile arama yapılamaz
                // var relatedPosts = await _blogPostTagRepository.GetByIdAsync(tag.Id);

                // Doğru kullanım (eğer gerekiyorsa):
                // var relatedPosts = await _blogPostTagRepository.GetByBlogTagIdAsync(tag.Id);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return tag.Id;
            }
            catch (DataBaseValidationException)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw; // Aynı exception'ı tekrar fırlat
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("Blog tag güncellenemedi", ex);
            }
        }
    }

}
