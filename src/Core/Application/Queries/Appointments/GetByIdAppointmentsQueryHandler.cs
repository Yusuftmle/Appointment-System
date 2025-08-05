using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Repositories.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Appointments
{
    public class GetByIdAppointmentsQueryHandler:IRequestHandler<GetByIdAppointmentsQuery, AppointmentDto>
    {
        private readonly IAppointmentRepository appointmentRepository;
       

        public GetByIdAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
        {
            this.appointmentRepository = appointmentRepository;
           
        }

        public async Task<AppointmentDto> Handle(GetByIdAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var query = appointmentRepository.AsQueryable();

            query = query
               .AsNoTracking()
               .Include(i => i.User)
               .Include(i => i.Service)
               .Where(i => i.Id == request.Id && !i.IsDeleted); // filtre önemli!

            var result = await query.Select(i => new AppointmentDto()
            {

                UserName = i.User.FullName,
                Id = i.Id,
                UserId = i.UserId,
                ServiceId = i.ServiceId,
                ServiceName = i.Service.Name,
                PhoneNumber = i.User.PhoneNumber,

                AppointmentDate = i.AppointmentDateTime,
                appointmentType = i.Type,


            }).FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
    
    
}
