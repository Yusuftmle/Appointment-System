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

namespace Application.RequestModels.TimeSlotCommand.Create
{
    public class AddAvailableTimeSlotCommandHandler : IRequestHandler<AddAvailableTimeSlotCommand, bool>
    {
        private readonly IAvailabilityRepository availabilityRepository;

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public AddAvailableTimeSlotCommandHandler(IAvailabilityRepository availabilityRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.availabilityRepository = availabilityRepository;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddAvailableTimeSlotCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();
                var isExist = await availabilityRepository.FirstOrDefaultAsync(i => i.AppointmentDateTime == request.AppointmentDateTime);
                if (isExist != null)
                {
                    return false;  // Saat zaten sistemde kayıtlı
                }

                var dbAvailability = mapper.Map<Domain.Models.Availability>(request);
                await availabilityRepository.AddAsync(dbAvailability);
                await unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch(Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("Servis Eklenemedi", ex);
            }
           
        }
    }
}
