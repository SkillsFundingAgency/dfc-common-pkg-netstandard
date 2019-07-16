using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace NCS.DSS.ActionPlan.Cosmos.Client
{
    public class CosmosDocumentClient : ICosmosDocumentClient
    {
        private readonly IDocumentClient _documentClient;

        public CosmosDocumentClient()
        {
            _documentClient = CreateDocumentClient();
        }

        private IDocumentClient CreateDocumentClient()
        {
            var connectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException();
            }

            var endPoint = connectionString.Split(new[] { "AccountEndpoint=" }, StringSplitOptions.None)[1]
                .Split(';')[0]
                .Trim();

            if (string.IsNullOrWhiteSpace(endPoint))
            {
                throw new ArgumentNullException();
            }

            var key = connectionString.Split(new[] { "AccountKey=" }, StringSplitOptions.None)[1]
                .Split(';')[0]
                .Trim();

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException();
            }

            return new DocumentClient(new Uri(endPoint), key);
        }

        public IDocumentClient GetDocumentClient()
        {
            return _documentClient;
        }
    }
}
