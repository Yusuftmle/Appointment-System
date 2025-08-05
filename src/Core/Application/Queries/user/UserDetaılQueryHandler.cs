using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.Queries.user;
using Application.Repositories.Interfaces;
using Domain.Models;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using HotelVR.Common.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.User
{
    public class UserDetailQueryHandler : IRequestHandler<UserDetaılQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserDetailQueryHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> Handle(UserDetaılQuery request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var query = _userRepository.AsQueryable()
                    .Include(i => i.Appointments)
                    .Where(i => i.Id == request.Id && !i.IsDeleted);

                var result = await query.Select(i => new UserDto
                {
                    Email = i.Email,
                    FullName = i.FullName,
                    PhoneNumber = i.PhoneNumber,
                    firstName = i.firstName,
                    lastName = i.lastName,
                    Appointment = i.Appointments
                }).FirstOrDefaultAsync(cancellationToken);

                if (result == null)
                    throw new DataBaseValidationException("Kullanıcı bulunamadı");

                await _unitOfWork.CommitTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new DataBaseValidationException($"Hata oluştu: {ex.Message}");
            }
        }
    }
}
