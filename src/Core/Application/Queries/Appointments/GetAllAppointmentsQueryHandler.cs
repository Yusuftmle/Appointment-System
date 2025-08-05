using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Repositories.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Appointments
{
    public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, List<AppointmentDto>>
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly IMapper mapper;

        public GetAllAppointmentsQueryHandler(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            this.appointmentRepository = appointmentRepository;
            this.mapper = mapper;
        }

        public async Task<List<AppointmentDto>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {

            var query = appointmentRepository.AsQueryable();

            query = query
                .AsNoTracking()
                .Include(i => i.User)
                .Include(i => i.Service)
                .OrderByDescending(i=>i.AppointmentDateTime)
                .Where(i => !i.IsDeleted);

            var result = await query
                .Select(i => new AppointmentDto()
                {
                    UserName = i.User.FullName,
                    Id = i.Id,
                    UserId = i.UserId,
                    ServiceId = i.ServiceId,
                    ServiceName = i.Service.Name,
                 
                    AppointmentDate = i.AppointmentDateTime,
                    
                    PhoneNumber  =i.User.PhoneNumber,
                    ReservationStatus = i.Status,
                    appointmentType = i.Type,

                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
