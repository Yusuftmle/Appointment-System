using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.Appointment.Delete
{
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, Guid>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork)
        {
            _appointmentRepository = appointmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var service = await _appointmentRepository.GetByIdAsync(request.Id);

                if (service == null)
                    throw new DataBaseValidationException("Blog Bulunamadi");

                // Soft delete işlemi
                service.IsDeleted = true;

                await _appointmentRepository.UpdateAsync(service);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return service.Id;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("Randevu silinirken bir hata oluştu.");
            }
        }
    }
}
