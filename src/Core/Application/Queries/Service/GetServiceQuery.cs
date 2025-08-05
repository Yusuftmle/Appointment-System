using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Queries.Service
{
    public class GetServiceQuery:IRequest<List<ServiceDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
