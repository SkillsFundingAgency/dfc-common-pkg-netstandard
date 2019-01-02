using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace DFC.Common.Core.Logging
{
    public static class LoggerExtensions
    {
        
        public static void LogHttpResponseMessage(this ILogger logger, Guid correlationId, string message, HttpResponseMessage httpResponseMessage)
        {
            CheckLoggerIsNotNull(logger);

            if (httpResponseMessage == null)
                throw new ArgumentNullException(nameof(httpResponseMessage));

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                logger.LogInformationObject(correlationId, message, httpResponseMessage);
            }
            else
            {
                logger.LogWarning("CorrelationId: {0} Message: {1} HttpResponseMessage: {2}", correlationId, message, JsonConvert.SerializeObject(httpResponseMessage));
            }
        }

        public static void LogInformationMessage(this ILogger logger, Guid correlationId, string message)
        {
            CheckLoggerIsNotNull(logger);

            logger.LogInformation(string.Format("CorrelationId: {0} Message: {1} ", correlationId, message));
        }

        public static void LogWarningMessage(this ILogger logger, Guid correlationId, string message)
        {
            CheckLoggerIsNotNull(logger);

            logger.LogWarning(string.Format("CorrelationId: {0} Message: {1} ", correlationId, message));
        }

        public static void LogError(this ILogger logger, Guid correlationId, Exception exception)
        {
            CheckLoggerIsNotNull(logger);

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            logger.LogError(correlationId, string.Empty, exception);
        }

        public static void LogError(this ILogger logger, Guid correlationId, string message, Exception exception)
        {
            CheckLoggerIsNotNull(logger);

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            logger.LogError("CorrelationId: {0}  Message: {1} Error: {2}", correlationId, message, exception);
        }

        public static void LogException(this ILogger logger, Guid correlationId, Exception exception)
        {
            CheckLoggerIsNotNull(logger);

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            logger.LogException(correlationId, string.Empty, exception);
        }

        public static void LogException(this ILogger logger, Guid correlationId, string message, Exception exception)
        {
            CheckLoggerIsNotNull(logger);

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            logger.LogError("CorrelationId: {0}  Message: {1} Exception: {2}", correlationId, message, exception);
        }

        public static void LogInformationObject(this ILogger logger, Guid correlationId, string message, object obj)
        {
            CheckLoggerIsNotNull(logger);

            string json;

            if (obj is string)
            {
                var maxLength = 4096; // This may change but it is a best guess for now.
                json = ((string)obj)?.Substring(0, Math.Min(((string)obj).Length, maxLength));
            }
            else
            {
                json = JsonConvert.SerializeObject(obj);
            }

            logger.LogInformation("CorrelationId: {0}  Message: {1} Object: {2}", correlationId, message, json);
        }

        public static void LogCosmosQueryRequestCharge(this ILogger logger, Guid correlationId, string message, double requestCharge)
        {
            logger.LogInformation("CorrelationId: {0}  Message: {1} Request Charge: {2}", correlationId, message, requestCharge);
        }

        public static void LogCosmosQueryMetricsObject(this ILogger logger, Guid correlationId, string message, QueryMetrics cosmosQueryMetrics)
        {
            logger.LogInformation("CorrelationId: {0}  Message: {1} Object: {2}", correlationId, message, JsonConvert.SerializeObject(cosmosQueryMetrics));
        }
        
        public static void LogCosmosQueryMetrics(this ILogger logger, Guid correlationId, string message, QueryMetrics cosmosQueryMetrics)
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

        public static void LogMethodEnter(this ILogger logger)
        {
            CheckLoggerIsNotNull(logger);

            if (logger.IsEnabled(LogLevel.Debug))
            {
                var method = new StackFrame(1).GetMethod();
                logger.LogDebug("Entering method. {0}", method?.DeclaringType?.ToString());
            }
        }

        public static void LogMethodExit(this ILogger logger)
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                var method = new StackFrame(1).GetMethod();
                logger.LogDebug("Exiting method. {0}", method?.DeclaringType?.ToString());
            }
        }

        private static void CheckLoggerIsNotNull(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

        }
    }
}
