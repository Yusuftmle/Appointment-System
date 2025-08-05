using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelRv.Infrastructure.Persistence.Repositories;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.BlogTag.Delete
{
    public class BlogTagDeleteCommandHandler : IRequestHandler<BlogTagDeleteCommand, Guid>
    {
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BlogTagDeleteCommandHandler(IBlogTagRepository blogTagRepository, IUnitOfWork unitOfWork)
        {
            _blogTagRepository = blogTagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(BlogTagDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var service = await _blogTagRepository.GetByIdAsync(request.Id);

                if (service == null)
                    throw new DataBaseValidationException("Tag bulunamadı.");

                // Soft delete işlemi
                service.IsDeleted = true;

                await _blogTagRepository.UpdateAsync(service);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return service.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("Tag silinirken bir hata oluştu.");
            }
        }
    }
}
