using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Repositories.Interfaces;
using AutoMapper;
using HotelRvDbContext.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Application.Queries.user
{
    public class AllUserLisQueryHandler : IRequestHandler<AllUserListQuery,List< UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper mapper;

        public AllUserLisQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(AllUserListQuery request, CancellationToken cancellationToken)
        {
            var User = await _userRepository.GetAll();
            return await Task.FromResult(mapper.Map<List<UserDto>>(User));
        }
    }
}
