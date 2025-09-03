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
using Microsoft.Extensions.Logging;

namespace Application.RequestModels.Appointment.UpdateAppoıntmentStatus
{
    public class ConfirmAppoıntmentStatusCommandHandler : IRequestHandler<ConfirmAppoıntmentStatusCommand, Guid>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConfirmAppoıntmentStatusCommandHandler> _logger; // Logger ekle

        public ConfirmAppoıntmentStatusCommandHandler(
            IAppointmentRepository appointmentRepository,
            IUnitOfWork unitOfWork,
            ILogger<ConfirmAppoıntmentStatusCommandHandler> logger)
        {
            _appointmentRepository = appointmentRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Guid> Handle(ConfirmAppoıntmentStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync().ConfigureAwait(false);

                var appointment = await _appointmentRepository.GetSingleAsync(
                    i => i.Id == request.Id && !i.IsDeleted,
                    cancellationToken: cancellationToken).ConfigureAwait(false);

                // 🔍 KONTROL 1: Appointment bulunamadı
                if (appointment == null)
                {
                    throw new NotFoundException($"Appointment with ID {request.Id} not found or has been deleted.");
                }

                // 🔍 KONTROL 2: Zaten confirm edilmiş mi? (İş kuralı)
                if (appointment.Status == Domain.Enums.ReservationStatus.Confirmed)
                {
                    throw new BusinessRuleException("Appointment is already confirmed.");
                }

                // 🔍 KONTROL 3: İptal edilmiş randevu confirm edilmeye çalışılıyor mu? (İş kuralı)
                if (appointment.Status == Domain.Enums.ReservationStatus.Canceled)
                {
                    throw new BusinessRuleException("Cannot confirm a canceled appointment.");
                }

                // 🔍 KONTROL 4: Tamamlanmış randevu confirm edilmeye çalışılıyor mu? (İş kuralı)
                if (appointment.Status == Domain.Enums.ReservationStatus.Confirmed)
                {
                    throw new BusinessRuleException("Cannot confirm a completed appointment.");
                }

                // 🔍 KONTROL 5: Geçmiş tarihli randevu confirm edilmeye çalışılıyor mu? (İş kuralı)
                if (appointment.AppointmentDateTime <= DateTime.UtcNow)
                {
                    throw new BusinessRuleException("Cannot confirm past appointments.");
                }

                // 🔍 KONTROL 6: Çok erken confirm (opsiyonel business rule)
                // Örnek: Randevu tarihinden 24 saat önce confirm edilebilir
                var timeDifference = appointment.AppointmentDateTime - DateTime.UtcNow;
                if (timeDifference.TotalHours < 1)
                {
                    throw new BusinessRuleException("Appointment can only be confirmed at least 1 hour before scheduled time.");
                }

                // ✅ İş mantığı
                appointment.Status = Domain.Enums.ReservationStatus.Confirmed;
               

                await _appointmentRepository.UpdateAsync(appointment).ConfigureAwait(false);
                await _unitOfWork.CommitTransactionAsync().ConfigureAwait(false);

                _logger.LogInformation("Appointment {AppointmentId} confirmed successfully", appointment.Id);
                return appointment.Id;
            }
            // 🎯 SPESİFİK EXCEPTION HANDLİNG
            catch (NotFoundException ex)
            {
                await _unitOfWork.RollbackTransactionAsync().ConfigureAwait(false);
                _logger.LogWarning(ex, "Appointment not found: {AppointmentId}", request.Id);
                throw; // Client'a anlamlı mesaj gitsin
            }
            catch (BusinessRuleException ex)
            {
                await _unitOfWork.RollbackTransactionAsync().ConfigureAwait(false);
                _logger.LogWarning(ex, "Business rule violation for appointment {AppointmentId}: {Message}",
                    request.Id, ex.Message);
                throw; // İş kuralı hatalarını olduğu gibi fırlat
            }
            catch (ConcurrencyException ex)
            {
                await _unitOfWork.RollbackTransactionAsync().ConfigureAwait(false);
                _logger.LogWarning(ex, "Concurrency conflict for appointment {AppointmentId}", request.Id);
                throw; // Concurrency hatalarını olduğu gibi fırlat
            }
            catch (DataBaseValidationException ex)
            {
                await _unitOfWork.RollbackTransactionAsync().ConfigureAwait(false);
                _logger.LogError(ex, "Database validation failed for appointment {AppointmentId}: {Message}",
                    request.Id, ex.Message);
                throw; // Database validation hatalarını olduğu gibi fırlat
            }
            catch (ExternalServiceException ex)
            {
                await _unitOfWork.RollbackTransactionAsync().ConfigureAwait(false);
                _logger.LogError(ex, "External service error for appointment {AppointmentId}: {Message}",
                    request.Id, ex.Message);
                throw new ExternalServiceException("External service unavailable. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync().ConfigureAwait(false);
                _logger.LogError(ex, "Unexpected error while confirming appointment {AppointmentId}", request.Id);
                throw new DataBaseValidationException("An unexpected error occurred while confirming the appointment.", ex);
            }
        }
    }
}