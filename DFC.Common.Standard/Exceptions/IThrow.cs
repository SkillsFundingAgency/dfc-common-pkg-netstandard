using System.Collections.Generic;

namespace DFC.Common.Standard.Exceptions
{
    public interface IThrow
    {
        void IfNull<T>(T argument, string name);
        void IfNullOrEmpty(string argument, string name);
        void IfNullOrEmpty<T>(IEnumerable<T> argument, string name);
        void IfNullOrWhiteSpace(string argument, string name);
        void IfLessThan(int limit, int argument, string name);
        void IfGreaterThan(int limit, int argument, string name);
        void IfLessThan(decimal limit, decimal argument, string name);
        void IfGreaterThan(decimal limit, decimal argument, string name);
    }
}
