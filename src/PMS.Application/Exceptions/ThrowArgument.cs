using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Application.Exceptions
{
    public class ThrowArgument
    {
        public static void NullExceptionIfNull(object argument)
        {
            if (argument == null)
            {
                throw new ArgumentNullException("Cannot be null", nameof(argument));
            }
        }

        public static void ExceptionIfZero(int id)
        {
            if (id == 0)
                throw new ArgumentException("ID cannot be zero.", nameof(id));
        }
    }
}