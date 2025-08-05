using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.TimeSlotCommand.Update
{
    public class UpdateAvailableTimeSlotCommandHandler : IRequestHandler<UpdateAvailableTimeSlotCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IAvailabilityRepository availabilityRepository;

        public UpdateAvailableTimeSlotCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IAvailabilityRepository availabilityRepository)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.availabilityRepository = availabilityRepository;
        }

        public async Task<Guid> Handle(UpdateAvailableTimeSlotCommand request, CancellationToken cancellationToken)
        {
            try
            {  
                await unitOfWork.BeginTransactionAsync();
                var existTimeslot = await availabilityRepository.GetByIdAsync(request.Id);
                if (existTimeslot == null)
                {
                  throw new DataBaseValidationException(" sistemde kayıtlı servis yok ");
                }

                var isExist = await availabilityRepository.FirstOrDefaultAsync(i => i.AppointmentDateTime == request.AppointmentDateTime);

                if (isExist != null)
                {
                    return Guid.Empty;  // Saat zaten sistemde kayıtlı
                }
                var dbAvailability = mapper.Map(request, existTimeslot);
                await availabilityRepository.UpdateAsync(dbAvailability);
                await unitOfWork.CommitTransactionAsync();

                return existTimeslot.Id;

            }
            catch (Exception ex) 
            {
                await unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("Zaten sistemde kayıtlı değil", ex);
            }
           
        }
    }
}
