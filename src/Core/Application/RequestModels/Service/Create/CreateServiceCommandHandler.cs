using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.Service.Create
{
    public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, Guid>
    {
        private readonly IServiceRepository serviceRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CreateServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.serviceRepository = serviceRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();
                var ExistingService = await serviceRepository.GetSingleAsync(x => x.Name == request.Name);
                if (ExistingService != null)
                {
                    throw new DataBaseValidationException("Bu isimde bir hizmet zaten mevcut");
                }


                var service = mapper.Map<Domain.Models.Service>(request);
                await serviceRepository.AddAsync(service);
                await unitOfWork.CommitTransactionAsync();
                return service.Id;
            }
            catch
            {
                await unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("Hizmet oluşturulamadı");
            }

        }
    }
}
