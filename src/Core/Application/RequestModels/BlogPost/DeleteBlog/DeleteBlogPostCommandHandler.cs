using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.BlogPost.DeleteBlog
{
    public class DeleteBlogPostCommandHandler : IRequestHandler<DeleteBlogPostCommand, Guid>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBlogPostCommandHandler(IBlogPostRepository blogPostRepository, IUnitOfWork unitOfWork)
        {
            _blogPostRepository = blogPostRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var service = await _blogPostRepository.GetByIdAsync(request.Id);

                if (service == null)
                    throw new DataBaseValidationException("Blog Bulunamadi");

                // Soft delete işlemi
                service.IsDeleted = true;

                await _blogPostRepository.UpdateAsync(service);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return service.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("Blog silinirken bir hata oluştu.");
            }
        }
    }
}
