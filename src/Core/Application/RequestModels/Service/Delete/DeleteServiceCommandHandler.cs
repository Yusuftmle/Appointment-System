using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.Service.Delete
{
    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, Guid>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
        {
            _serviceRepository = serviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var service = await _serviceRepository.GetByIdAsync(request.Id);

                if (service == null)
                    throw new DataBaseValidationException("Servis bulunamadı.");

                // Soft delete işlemi
                service.IsDeleted = true;

                await _serviceRepository.UpdateAsync(service);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return service.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("Servis silinirken bir hata oluştu.");
            }
        }
    }


}
