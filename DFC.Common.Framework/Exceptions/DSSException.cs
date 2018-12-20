using System;

namespace DFC.Common.Framework.Exceptions
{
    public class DssException : Exception
    {
        public DssException()
            : base() { }

        public DssException(string message)
            : base(message) { }

        public DssException(string format, DssExceptionMessage args)
            : base(string.Format(format, args)) { }

        public DssException(string message, Exception innerException)
            : base(message, innerException) { }

        public DssException(string format, Exception innerException, DssExceptionMessage args)
            : base(string.Format(format, args), innerException) { }
    }
}