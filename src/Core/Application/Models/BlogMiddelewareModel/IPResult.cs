using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.BlogMiddelewareModel
{
    public class IPResult
    {
        public bool IsDetected { get; set; }
        public string BotType { get; set; } = string.Empty;
    }
}
