using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Extension;
using Application.Models;
using Application.Models.Page;
using Application.Repositories.Interfaces;
using Domain.Enums;
using Domain.Models;
using MediatR;

namespace Application.Queries.Appointments
{
    public class GetPaginatedAppointmentsQueryHandler : IRequestHandler<GetPaginatedAppointmentsQuery, PagedViewModel<AppointmentDto>>
    {
        private readonly IAppointmentRepository AppointmentRepository;

        public GetPaginatedAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
        {
            AppointmentRepository = appointmentRepository;
        }

        public async Task<PagedViewModel<AppointmentDto>> Handle(GetPaginatedAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var query = AppointmentRepository.AsQueryable()
                        .AsNoTracking()
                        .Include(i => i.User)
                        .Include(i => i.Service)
                        .Where(i => !i.IsDeleted);

            // Eğer UserId varsa filtre uygula
            if (request.UserId.HasValue)
            {
                query = query.Where(i => i.UserId == request.UserId.Value);
            }

            query = query.OrderByDescending(i => i.AppointmentDateTime);

            var list = query.Select(i => new AppointmentDto()
            {
                UserName = i.User.FullName,
                Id = i.Id,
                UserId = i.UserId,
                ServiceId = i.ServiceId,
                ServiceName = i.Service.Name,
                AppointmentDate = i.AppointmentDateTime,
                PhoneNumber = i.User.PhoneNumber,
                ReservationStatus = i.Status,
                appointmentType = i.Type,



            });

            var entries = await list.GetPaged(request.Page, request.pageSize);

            return entries;
        }

    }
}
