using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;

namespace DFC.Common.Standard.CosmosDocumentClient
{
    public class CosmosDocumentClient : ICosmosDocumentClient
    {
        private readonly IDocumentClient _documentClient;
        private readonly ILogger _logger;

        public CosmosDocumentClient(ILogger logger)
        {
            _logger = logger;
            _documentClient = CreateDocumentClient();  
        }

        private IDocumentClient CreateDocumentClient()
        {
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
                var endPoint = connectionString.Split(new[] { "AccountEndpoint=" }, StringSplitOptions.None)[1]
                    .Split(';')[0]
                    .Trim();

                var key = connectionString.Split(new[] { "AccountKey=" }, StringSplitOptions.None)[1]
                    .Split(';')[0]
                    .Trim();

                return new DocumentClient(new Uri(endPoint), key);
            }
            catch (ArgumentNullException ex )
            { 
                throw ex;
            }            
        }

        public IDocumentClient GetDocumentClient()
        {
            return _documentClient;
        }
    }
}
