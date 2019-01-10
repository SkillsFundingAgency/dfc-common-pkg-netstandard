using System;
using System.Net.Http;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Logging;

namespace DFC.Common.Standard.Logging
{
    public interface ILoggerHelper
    {
        void LogHttpResponseMessage(ILogger logger, Guid correlationId, string message,
            HttpResponseMessage httpResponseMessage);
        void LogInformationMessage(ILogger logger, Guid correlationId, string message);
        void LogWarningMessage(ILogger logger, Guid correlationId, string message);
        void LogError(ILogger logger, Guid correlationId, Exception exception);
        void LogError(ILogger logger, Guid correlationId, string message, Exception exception);
        void LogException(ILogger logger, Guid correlationId, Exception exception);
        void LogException(ILogger logger, Guid correlationId, string message, Exception exception);
        void LogInformationObject(ILogger logger, Guid correlationId, string message, object obj);
        void LogCosmosQueryRequestCharge(ILogger logger, Guid correlationId, string message, double requestCharge);
        void LogCosmosQueryMetricsObject(ILogger logger, Guid correlationId, string message,
            QueryMetrics cosmosQueryMetrics);
        void LogCosmosQueryMetrics(ILogger logger, Guid correlationId, string message, QueryMetrics cosmosQueryMetrics);
        void LogMethodEnter(ILogger logger);
        void LogMethodExit(ILogger logger);
    }
}
