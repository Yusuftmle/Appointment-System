using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Application.Queries.Service
{
    public class GetServiceQueryHandler : IRequestHandler<GetServiceQuery, List<ServiceDto>>
    {
        private readonly IMapper mapper;
        private readonly IServiceRepository _serviceRepository;

        public GetServiceQueryHandler(IMapper mapper, IServiceRepository serviceRepository)
        {
            this.mapper = mapper;
            _serviceRepository = serviceRepository;
        }

        public async Task<List<ServiceDto>> Handle(GetServiceQuery request, CancellationToken cancellationToken)
        {
           var services = await _serviceRepository.GetAll();
            return await Task.FromResult(mapper.Map<List<ServiceDto>>(services));
        }
    }
}
