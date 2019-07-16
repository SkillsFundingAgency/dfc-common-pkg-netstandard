using System;

namespace DFC.Common.Standard.GuidHelper
{
    public class GuidHelper : IGuidHelper
    {
        public bool IsValidGuid(string id)
        {
            return Guid.TryParse(id, out _);
        }

        public Guid ValidateGuid(string id)
        {
            return Guid.TryParse(id, out var validGuid) ? validGuid : Guid.Empty;
        }

        public Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }
    }
}
