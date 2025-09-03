using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelVR.Common.Infrastructure.Exceptions
{
    public class ExternalServiceException:Exception
    {
        public ExternalServiceException()
        {
        }
        public ExternalServiceException(string message) : base(message)
        {
        }
        public ExternalServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
