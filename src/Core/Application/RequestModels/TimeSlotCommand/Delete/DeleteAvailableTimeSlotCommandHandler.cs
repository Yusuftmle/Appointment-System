using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.TimeSlotCommand.Delete
{
    public class DeleteAvailableTimeSlotCommandHandler : IRequestHandler<DeleteAvailableTimeSlotCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAvailabilityRepository _availableTimeSlotRepository;

        public DeleteAvailableTimeSlotCommandHandler(IUnitOfWork unitOfWork, IAvailabilityRepository availableTimeSlotRepository)
        {
            _unitOfWork = unitOfWork;
            _availableTimeSlotRepository = availableTimeSlotRepository;
        }

        public async Task<Guid> Handle(DeleteAvailableTimeSlotCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var service = await _availableTimeSlotRepository.GetByIdAsync(request.Id);

                if (service == null)
                    throw new DataBaseValidationException("Uygun Zaman bulunamadı.");

                // Soft delete işlemi
                service.IsDeleted = true;

                await _availableTimeSlotRepository.UpdateAsync(service);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return service.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("Uygun Zaman dilimi silinirken bir hata olustu.");
            }
        }
    }
}
