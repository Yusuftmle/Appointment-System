﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelVR.Common.Infrastructure.Exceptions
{
    public class BusinessRuleException: Exception
    {
        public BusinessRuleException()
        {
        }
        public BusinessRuleException(string? message) : base(message)
        {
        }
        public BusinessRuleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
