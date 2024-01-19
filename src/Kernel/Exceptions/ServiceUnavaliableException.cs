using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Exceptions
{
    public class ServiceUnavaliableException : Exception
    {
        public ServiceUnavaliableException()
        {
        }

        public ServiceUnavaliableException(string? message) : base(message)
        {
        }
    }
}
