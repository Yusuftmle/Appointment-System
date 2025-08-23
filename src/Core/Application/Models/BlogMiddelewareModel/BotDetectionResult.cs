using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.BlogMiddelewareModel
{
    public class BotDetectionResult
    {
        public bool IsBot { get; set; }
        public string BotType { get; set; } = string.Empty;
        public int Confidence { get; set; }
        public string DetectionMethod { get; set; } = string.Empty;
    }
}
