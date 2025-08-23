using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.BlogMiddelewareModel
{
    public class ClientInfo
    {
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string Referrer { get; set; } = string.Empty;
        public string AcceptHeader { get; set; } = string.Empty;
        public string AcceptLanguage { get; set; } = string.Empty;
        public string AcceptEncoding { get; set; } = string.Empty;
        public DateTime RequestTime { get; set; }
        public bool HasJavaScriptCapability { get; set; }
    }
}
