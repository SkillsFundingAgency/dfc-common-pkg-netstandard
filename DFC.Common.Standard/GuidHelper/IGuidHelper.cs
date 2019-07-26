using System;

namespace DFC.Common.Standard.GuidHelper
{
    public interface IGuidHelper
    {

        bool IsValidGuid(string id);

        Guid ValidateAndGetGuid(string id);

        Guid GenerateGuid();
    }
}
