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
using Microsoft.Extensions.Logging;

namespace Application.RequestModels.Appointment.Create
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        private readonly IMapper mapper;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateAppointmentCommandHandler> _logger;

        public CreateAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IMapper mapper, IUnitOfWork unitOfWork, IServiceRepository serviceRepository, ILogger<CreateAppointmentCommandHandler> logger)
        {
            _appointmentRepository = appointmentRepository;

            this.mapper = mapper;
            _unitOfWork = unitOfWork;
            _serviceRepository = serviceRepository;
            _logger = logger;
        }
        // added .ConfigureAwait(false) 
        public async Task<Guid> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync().ConfigureAwait(false);

            try
            {
                var existAppointment = await _appointmentRepository.GetSingleAsync(
                 i => i.AppointmentDateTime == request.AppointmentDateTime,true,
                   cancellationToken).ConfigureAwait(false);

                if (existAppointment != null)
                    throw new DataBaseValidationException("Appointment already exist");

                var service = await _serviceRepository.GetByIdAsync(request.ServiceId).ConfigureAwait(false);
                if (service == null)
                    throw new DataBaseValidationException("Service not found");

               

                var dbAppointment = mapper.Map<Domain.Models.Appointment>(request);

                await _appointmentRepository.AddAsync(dbAppointment).ConfigureAwait(false);
                await _unitOfWork.CommitTransactionAsync().ConfigureAwait(false);
                return dbAppointment.Id;
            }
            catch (DataBaseValidationException ex)
            {
                await _unitOfWork.RollbackTransactionAsync().ConfigureAwait(false);
                _logger.LogWarning(ex, "Validation hatası oluştu: {Message}", ex.Message);
                throw new DataBaseValidationException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync().ConfigureAwait(false);
                _logger.LogError(ex, "Bilinmeyen hata oluştu: {Message}", ex.Message);
                throw new Exception("An error occurred while creating the appointment.", ex);
            }

        }
    }
}
