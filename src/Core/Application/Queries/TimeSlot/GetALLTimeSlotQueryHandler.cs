using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;

namespace Application.Queries.TimeSlot
{
    public class GetALLTimeSlotQueryHandler : IRequestHandler<GetALLTimeSlotQuery, List<TimeSlotDto>>
    {

        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IMapper _mapper;

        public GetALLTimeSlotQueryHandler(IAvailabilityRepository availabilityRepository, IMapper mapper)
        {
            _availabilityRepository = availabilityRepository;
            _mapper = mapper;
        }

        public async Task<List<TimeSlotDto>> Handle(GetALLTimeSlotQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Tüm time slot'ları al
                var timeSlots = await _availabilityRepository.GetAll();

               
                var timeSlotDtos = _mapper.Map<List<TimeSlotDto>>(timeSlots);
                return timeSlotDtos;
            }
            catch (DataBaseValidationException ex)
            {
                throw;
            }
        }
    }
}
