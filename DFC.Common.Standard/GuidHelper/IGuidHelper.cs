using System;

namespace DFC.Common.Standard.GuidHelper
{
    public interface IGuidHelper
    {

        bool IsValidGuid(string id);

        Guid ValidateGuid(string id);

        Guid GenerateGuid();

    }
}
