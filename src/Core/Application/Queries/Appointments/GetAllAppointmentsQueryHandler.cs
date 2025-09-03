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
    public class GetAllAppointmentsQueryHandler_Optimized : IRequestHandler<GetAllAppointmentsQuery, List<AppointmentDto>>
    {
        private readonly IAppointmentRepository appointmentRepository;
       

        public GetAllAppointmentsQueryHandler_Optimized(IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
        }

        public async Task<List<AppointmentDto>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
        {
            // ✅ Include'lar kaldırıldı - Select otomatik join yapacak
            var result = await appointmentRepository.AsQueryable()
                .AsNoTracking()
                .Where(i => !i.IsDeleted)                    // ✅ Where önce - index kullanımı
                .OrderByDescending(i => i.AppointmentDateTime)
                .Select(i => new AppointmentDto()            // ✅ EF Core otomatik join yapacak
                {
                    UserName = i.User.FullName,              // JOIN Users ON...
                    Id = i.Id,
                    UserId = i.UserId,
                    ServiceId = i.ServiceId,
                    ServiceName = i.Service.Name,            // JOIN Services ON...
                    AppointmentDate = i.AppointmentDateTime,
                    PhoneNumber = i.User.PhoneNumber,
                    ReservationStatus = i.Status,
                    appointmentType = i.Type,
                })
                .ToListAsync(cancellationToken);

            return result;

            
        }
    }
}
