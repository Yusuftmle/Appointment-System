using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Application.Queries.TimeSlot
{
    public class TimeSlotQueryHandler : IRequestHandler<TimeSlotQuery, List<TimeSlotDto>>
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IMapper _mapper;

        public TimeSlotQueryHandler(IAvailabilityRepository availabilityRepository, IMapper mapper)
        {
            _availabilityRepository = availabilityRepository;
            _mapper = mapper;
        }

        public async Task<List<TimeSlotDto>> Handle(TimeSlotQuery request, CancellationToken cancellationToken)
        {
            // Belirli güne göre tüm slotları çekiyoruz
            var timeSlots = await _availabilityRepository.GetByDateAsync(request.Date);
            return _mapper.Map<List<TimeSlotDto>>(timeSlots);
        }
    }

}
