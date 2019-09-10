using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using NCS.DSS.CosmosDocumentClient.Interfaces;
using Newtonsoft.Json.Linq;

namespace NCS.DSS.CosmosDocumentClient
{
    public class CosmosProvider<T> where T : class, ICosmosProvider<T>
    {      
        private readonly IDocumentClient _documentClient;
        private readonly Uri _customerDocumentCollectionUri;
        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly string _customerDatabaseId;
        private readonly string _customerCollectionId;

        public CosmosProvider(IDocumentClient documentClient)
        {
            _documentClient = documentClient;

            _databaseId = Environment.GetEnvironmentVariable("DatabaseId");
            _collectionId = Environment.GetEnvironmentVariable("CollectionId");
            _customerDatabaseId = Environment.GetEnvironmentVariable("CustomerDatabaseId");
            _customerCollectionId = Environment.GetEnvironmentVariable("CustomerCollectionId");
        }

        public async Task<bool> DoesCustomerResourceExist(Guid customerId)
        {
            var documentUri = DocumentDBUrlHelper.CreateCustomerDocumentUri(customerId);

            try
            {
                
                var response = await _documentClient.ReadDocumentAsync(documentUri);
                if (response.Resource != null)
                {
                    return true;
                }
            }
            catch (DocumentClientException)
            {
                return false;
            }

            return false;
        }

        public async Task<bool> DoesCustomerHaveTerminationDate(Guid customerId)
        {
            var documentUri = DocumentDBUrlHelper.CreateCustomerDocumentUri(customerId);

            try
            {
                
                var response = await _documentClient.ReadDocumentAsync(documentUri);
                var dateOfTermination = response.Resource?.GetPropertyValue<DateTime?>("DateOfTermination");

                return dateOfTermination.HasValue;
            }
            catch (DocumentClientException)
            {
                return false;
            }
        }

        public bool DoesChildResourceExistForCustomer(Guid customerId)
        {
            var collectionUri = DocumentDBUrlHelper.CreateDocumentCollectionUri();
            

            if (_documentClient == null)
            {
                return false;
            }

            var employmentProgressionForCustomerQuery = _documentClient.CreateDocumentQuery<T>(collectionUri, new FeedOptions { MaxItemCount = 1 });
            var result = employmentProgressionForCustomerQuery.Where(x => x.CustomerId == customerId).AsEnumerable().Any();

            return result;
        }

        public async Task<T> GetChildResourceForCustomerAsync(Guid customerId, Guid childResourceId)
        {
            var collectionUri = CreateDocumentCollectionUri();
            
            var employmentProgressionForCustomerQuery = _documentClient
                ?.CreateDocumentQuery<T>(collectionUri, new FeedOptions { MaxItemCount = 1 })
                .Where(x => x.CustomerId == customerId && x.EmploymentProgressionId == childResourceId)
                .AsDocumentQuery();

            if (employmentProgressionForCustomerQuery == null)
            {
                return null;
            }

            var childResource = await employmentProgressionForCustomerQuery.ExecuteNextAsync<T>();

            return childResource?.FirstOrDefault();
        }

        public async Task<List<T>> GetChildResourceForCustomerAsync(Guid customerId)
        {
            var collectionUri = CreateDocumentCollectionUri();

            var childrenResourcesQuery = _documentClient.CreateDocumentQuery<T>(collectionUri)
                .Where(so => so.CustomerId == customerId).AsDocumentQuery();

            var employmentProgressions = new List<T>();

            while (childrenResourcesQuery.HasMoreResults)
            {
                var response = await childrenResourcesQuery.ExecuteNextAsync<T>();
                employmentProgressions.AddRange(response);
            }

            return employmentProgressions.Any() ? employmentProgressions : null;
        }

        public async Task<ResourceResponse<Document>> CreateChildResourceAsync(T childResource)
        {
            var collectionUri = CreateDocumentCollectionUri();
            
            var response = await _documentClient.CreateDocumentAsync(collectionUri, childResource);

            return response;
        }

        public async Task<ResourceResponse<Document>> UpdateResourceAsync(string childObjectAsJson, Guid childResourceId)
        {
            if (string.IsNullOrEmpty(childObjectAsJson))
            {
                return null;
            }

            var documentUri = CreateDocumentUri(childResourceId);

            if (_documentClient == null)
            {
                return null;
            }

            var childResourceDocumentAsJObject = JObject.Parse(childObjectAsJson);
            var response = await _documentClient.ReplaceDocumentAsync(documentUri, childResourceDocumentAsJObject);

            return response;
        }

        public async Task<string> GetChildResourceForCustomerToPatchAsync(Guid customerId, Guid childResourceId)
        {
            var collectionUri = CreateDocumentCollectionUri();
            

            var employmentProgressionQuery = _documentClient
                ?.CreateDocumentQuery<T>(collectionUri, new FeedOptions { MaxItemCount = 1 })
                    .Where(x => x.CustomerId == customerId && x.EmploymentProgressionId == childResourceId)
                    .AsDocumentQuery();

            if (employmentProgressionQuery == null)
            {
                return null;
            }

            var employmentProgressions = await employmentProgressionQuery.ExecuteNextAsync();
            return employmentProgressions?.FirstOrDefault()?.ToString();
        }
    }
}