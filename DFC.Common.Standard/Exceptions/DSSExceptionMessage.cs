using System;

namespace DFC.Common.Standard.Exceptions
{
    public class DssExceptionMessage
    {
        public DssExceptionMessage(Guid correlationId, string touchpointId, string url)
        {
            CorrelationId = correlationId;
            TouchpointId = touchpointId;
            Url = url;
        }

        public Guid CorrelationId { get; set; }
        public string TouchpointId { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return string.Format("CorrelationId: {0} TouchpointId:{1} Url:{2}", CorrelationId + Environment.NewLine, TouchpointId, Url + Environment.NewLine);
        }
    }
}