using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.RequestModels.Service.Update
{
    public class UpdateServiceCommandHandler:IRequestHandler<UpdateServiceCommand, Guid>
    {
        private readonly IServiceRepository serviceRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UpdateServiceCommandHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.serviceRepository = serviceRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();
                var dbService = await serviceRepository.GetByIdAsync(request.Id);
                if (dbService == null)
                {
                    throw new DataBaseValidationException("Service not found");
                }
               
                mapper.Map(request, dbService);
               
                var rows = await serviceRepository.UpdateAsync(dbService);
                await unitOfWork.CommitTransactionAsync();
                
                return dbService.Id;
            }
            catch
            {
                await unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException("Hizmet güncellenemedi");
            }
           
        }
    }
    
    
}
