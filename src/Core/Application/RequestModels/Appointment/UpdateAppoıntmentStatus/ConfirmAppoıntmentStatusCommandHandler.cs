using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories.Interfaces;
using AutoMapper;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.Appointment.UpdateAppoıntmentStatus
{
    public class ConfirmAppoıntmentStatusCommandHandler : IRequestHandler<ConfirmAppoıntmentStatusCommand, Guid>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUnitOfWork _unitOfWork;
      

        public ConfirmAppoıntmentStatusCommandHandler(IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork)
        {
            _appointmentRepository = appointmentRepository;
            _unitOfWork = unitOfWork;
           
        }

        public async Task<Guid> Handle(ConfirmAppoıntmentStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appointment = await _appointmentRepository.GetSingleAsync(i => i.Id == request.Id && !i.IsDeleted);

                if (appointment == null)
                    throw new DataBaseValidationException("Appointment not found or has been deleted.");

                appointment.Status = Domain.Enums.ReservationStatus.Confirmed;

                await _appointmentRepository.UpdateAsync(appointment);
                await _unitOfWork.SaveChangesAsync(); // Bu satır eklenebilir
                await _unitOfWork.CommitTransactionAsync();

                return appointment.Id;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("An error occurred while confirming the appointment status.", ex);
            }

        }
    }
}
