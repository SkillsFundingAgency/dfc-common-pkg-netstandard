using System;

namespace DFC.Common.Standard.GuidHelper
{
    public class GuidHelper : IGuidHelper
    {
        public bool IsValidGuid(string id)
        {
            if (string.IsNullOrEmpty(id?.Trim()))
            {
                return false;
            }

            return Guid.TryParse(id, out _);
        }

        public Guid ValidateAndGetGuid(string id)
        {
            if (string.IsNullOrEmpty(id?.Trim()))
            {
                return Guid.NewGuid();
            }

            return Guid.TryParse(id, out var validGuid) ? validGuid : Guid.NewGuid();
        }

        public Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }
    }
}
