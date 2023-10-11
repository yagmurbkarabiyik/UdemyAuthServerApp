using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibarary.Exceptions
{
    public class CustomExceptions : Exception
    {
        public CustomExceptions()
        {

        }
        public CustomExceptions(string message) : base(message)
        {
        }

        public CustomExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
