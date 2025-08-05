using System;
using System.Collections.Generic;
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
    public class CancelAppointmentStatusCommandHandler : IRequestHandler<CancelAppointmentStatusCommand, Guid>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CancelAppointmentStatusCommandHandler(IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CancelAppointmentStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var appointment = await _appointmentRepository.GetSingleAsync(i => i.Id == request.Id && !i.IsDeleted);

                if (appointment == null)
                    throw new DataBaseValidationException("Appointment not found or has been deleted.");

                appointment.Status = Domain.Enums.ReservationStatus.Canceled;

                await _appointmentRepository.UpdateAsync(appointment);
                await _unitOfWork.SaveChangesAsync(); // Bu satır eklenebilir
                await _unitOfWork.CommitTransactionAsync();

                return appointment.Id;
            }
            catch (DataBaseValidationException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("An error occurred while canceling the appointment.", ex);
            }
        }
    }
}
