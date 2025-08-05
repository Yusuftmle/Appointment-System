using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using MediatR;

namespace Application.Queries.TimeSlot
{
    public class TimeSlotQuery : IRequest<List<TimeSlotDto>>
    {
        public DateTime Date { get; set; }  // sadece tarih alıyoruz, saat frontend'de seçilecek

        public TimeSlotQuery(DateTime date)
        {
            Date = date;
        }
    }

}
