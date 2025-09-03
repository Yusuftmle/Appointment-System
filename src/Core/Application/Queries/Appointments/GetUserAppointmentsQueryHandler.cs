using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Appointments
{
    public class GetUserAppointmentsQueryHandler : IRequestHandler<GetUserAppointmentsQuery,List< AppointmentDto>>
    {
        private readonly IAppointmentRepository appointmentRepository;

        public GetUserAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
        }

        public async Task<List<AppointmentDto>> Handle(GetUserAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var query = appointmentRepository.AsQueryable();

            query = query
                .AsNoTracking()
                .Where(i => i.UserId == request.UserId && !i.IsDeleted);

            var result = await query
                .Select(i => new AppointmentDto()
                {
                    UserName = i.User.FullName,
                    Id = i.Id,
                    UserId = i.UserId,
                    ServiceId = i.ServiceId,
                    ServiceName = i.Service.Name,
                  
                    AppointmentDate = i.AppointmentDateTime,
                    IsConfirmed = i.Status == Domain.Enums.ReservationStatus.Confirmed,
                    appointmentType = i.Type,
                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
