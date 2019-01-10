using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace DFC.Common.Standard.Logging
{
    public class LoggerHelper : ILoggerHelper
    {
        
        public void LogHttpResponseMessage(ILogger logger, Guid correlationId, string message, HttpResponseMessage httpResponseMessage)
        {
            CheckLoggerIsNotNull(logger);

            if (httpResponseMessage == null)
                throw new ArgumentNullException(nameof(httpResponseMessage));

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                LogInformationObject(logger,correlationId, message, httpResponseMessage);
            }
            else
            {
                logger.LogWarning("CorrelationId: {0} Message: {1} HttpResponseMessage: {2}", correlationId, message, JsonConvert.SerializeObject(httpResponseMessage));
            }
        }

        public void LogInformationMessage(ILogger logger, Guid correlationId, string message)
        {
            CheckLoggerIsNotNull(logger);

            logger.LogInformation(string.Format("CorrelationId: {0} Message: {1} ", correlationId, message));
        }

        public void LogWarningMessage(ILogger logger, Guid correlationId, string message)
        {
            CheckLoggerIsNotNull(logger);

            logger.LogWarning(string.Format("CorrelationId: {0} Message: {1} ", correlationId, message));
        }

        public void LogError(ILogger logger, Guid correlationId, Exception exception)
        {
            CheckLoggerIsNotNull(logger);

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            LogError(logger, correlationId, string.Empty, exception);
        }

        public void LogError(ILogger logger, Guid correlationId, string message, Exception exception)
        {
            CheckLoggerIsNotNull(logger);

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            logger.LogError("CorrelationId: {0}  Message: {1} Error: {2}", correlationId, message, exception);
        }

        public void LogException(ILogger logger, Guid correlationId, Exception exception)
        {
            CheckLoggerIsNotNull(logger);

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            LogException(logger, correlationId, string.Empty, exception);
        }

        public void LogException(ILogger logger, Guid correlationId, string message, Exception exception)
        {
            CheckLoggerIsNotNull(logger);

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            logger.LogError("CorrelationId: {0}  Message: {1} Exception: {2}", correlationId, message, exception);
        }

        public void LogInformationObject(ILogger logger, Guid correlationId, string message, object obj)
        {
            CheckLoggerIsNotNull(logger);

            string json;

            if (obj is string)
            {
                var maxLength = 4096; //  may change but it is a best guess for now.
                json = ((string)obj)?.Substring(0, Math.Min(((string)obj).Length, maxLength));
            }
            else
            {
                json = JsonConvert.SerializeObject(obj);
            }

            logger.LogInformation("CorrelationId: {0}  Message: {1} Object: {2}", correlationId, message, json);
        }

        public void LogCosmosQueryRequestCharge(ILogger logger, Guid correlationId, string message, double requestCharge)
        {
            logger.LogInformation("CorrelationId: {0}  Message: {1} Request Charge: {2}", correlationId, message, requestCharge);
        }

        public void LogCosmosQueryMetricsObject(ILogger logger, Guid correlationId, string message, QueryMetrics cosmosQueryMetrics)
        {
            logger.LogInformation("CorrelationId: {0}  Message: {1} Object: {2}", correlationId, message, JsonConvert.SerializeObject(cosmosQueryMetrics));
        }
        
        public void LogCosmosQueryMetrics(ILogger logger, Guid correlationId, string message, QueryMetrics cosmosQueryMetrics)
        {
            logger.LogInformation("CorrelationId: {0}\n" +
                                  "Message: {1}\n" +
                                  "IndexHitRatio: {2}\n" +
                                  "OutputDocumentCount: {3}\n" +
                                  "Retries: {4}\n" +
                                  "RetrievedDocumentCount: {5}\n" +
                                  "TotalTime: {6}",
                correlationId,
                message,
                cosmosQueryMetrics.IndexHitRatio,
                cosmosQueryMetrics.OutputDocumentCount,
                cosmosQueryMetrics.Retries,
                cosmosQueryMetrics.RetrievedDocumentCount,
                cosmosQueryMetrics.TotalTime);
        }

        public void LogMethodEnter(ILogger logger)
        {
            CheckLoggerIsNotNull(logger);

            if (logger.IsEnabled(LogLevel.Debug))
            {
               var method = new StackFrame(1).GetMethod();
                logger.LogDebug("Entering method. {0}", method?.DeclaringType?.ToString());
            }
        }

        public void LogMethodExit(ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
               var method = new StackFrame(1).GetMethod();
                logger.LogDebug("Exiting method. {0}", method?.DeclaringType?.ToString());
            }
        }

        private void CheckLoggerIsNotNull(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

        }
    }
}
