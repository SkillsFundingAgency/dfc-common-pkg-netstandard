using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.Common.Standard.CosmosClient.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using NCS.DSS.CosmosDocumentClient.Interfaces;
using Newtonsoft.Json.Linq;

namespace NCS.DSS.CosmosDocumentClient
{
    public class CosmosClient<T> :ICosmosClient<T> where T : class, new()
    {
        private readonly IDocumentClient _documentClient;
        private readonly CosmosClientConfiguration _cosmosProviderConfiguration;
        private readonly Uri _collectionUri;

        public CosmosClient(IDocumentClient documentClient, CosmosClientConfiguration cosmosClientConfiguration)
        {
            _documentClient = documentClient;
            _cosmosProviderConfiguration = cosmosClientConfiguration;

            _collectionUri = CreateCollectionUri();
        }

        public async Task<T> GetResource(string resourceId)
        {
            return await _documentClient.ReadDocumentAsync<T>(CreateResourceDocumentUri(resourceId));
        }

        public T GetResource(SqlQuerySpec sqlQuerySpec)
        {
            var query = _documentClient.CreateDocumentQuery<T>(_collectionUri, sqlQuerySpec, new FeedOptions { MaxItemCount = 1 });

            return query.ToList().FirstOrDefault();
        }

        public List<T> GetResources(SqlQuerySpec sqlQuerySpec)
        {
            var query = _documentClient.CreateDocumentQuery<T>(_collectionUri, sqlQuerySpec, new FeedOptions { MaxItemCount = 1 });

            return query.ToList();
        }

        public async Task<ResourceResponse<Document>> CreateResourceAsync(T resource)
        {
            return await _documentClient.CreateDocumentAsync(_collectionUri, resource);
        }

        public async Task<ResourceResponse<Document>> UpdateResourceAsync(string jsonString, string resourceId)
        {
            var childResourceDocumentAsJObject = JObject.Parse(jsonString);

            return await _documentClient.ReplaceDocumentAsync(CreateDocumentUri(resourceId), childResourceDocumentAsJObject);
        }
       

        private Uri CreateDocumentUri(string resourceId)
        {
            return UriFactory.CreateDocumentUri(_cosmosProviderConfiguration.DatabaseId, _cosmosProviderConfiguration.CollectionId, resourceId);
        }

        private Uri CreateCollectionUri()
        {
            return UriFactory.CreateDocumentCollectionUri(_cosmosProviderConfiguration.DatabaseId, _cosmosProviderConfiguration.CollectionId);
        }

        private Uri CreateResourceDocumentUri(string resourceId)
        {
            return UriFactory.CreateDocumentUri(_cosmosProviderConfiguration.CustomerDatabaseId, _cosmosProviderConfiguration.CustomerCollectionId, resourceId);
        }
    }
}