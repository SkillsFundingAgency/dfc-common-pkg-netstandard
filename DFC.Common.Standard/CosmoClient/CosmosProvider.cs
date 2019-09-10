using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json.Linq;

namespace NCS.DSS.CosmosDocumentClient
{
    public class CosmosProvider<T> where T : class
    {
        private readonly IDocumentClient _documentClient;
        private readonly Uri _customerDocumentCollectionUri;
        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly string _customerDatabaseId;
        private readonly string _customerCollectionId;

        private Uri _documentCollectionUri;

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
            var documentUri = CreateCustomerDocumentUri(customerId);

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

        public Uri CreateCustomerDocumentUri(Guid customerId)
        {
            return UriFactory.CreateDocumentUri(_customerDatabaseId, _customerCollectionId, customerId.ToString());
        }

        public async Task<bool> DoesCustomerHaveTerminationDate(Guid customerId)
        {
            var documentUri = CreateCustomerDocumentUri(customerId);

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

        public bool DoesChildResourceExistForCustomer(Guid customerId, string customerPropertyName)
        {
            var collectionUri = CreateDocumentCollectionUri();

            var employmentProgressionForCustomerQuery = _documentClient.CreateDocumentQuery<T>(collectionUri, new FeedOptions { MaxItemCount = 1 });
            var result = employmentProgressionForCustomerQuery.Where(x => Guid.Parse(x.GetType().GetProperty(nameof(customerPropertyName)).GetValue(nameof(customerPropertyName)).ToString()) == customerId).AsEnumerable().Any();

            return result;
        }

        public Uri CreateDocumentCollectionUri()
        {
            if (_documentCollectionUri != null)
            {
                return _documentCollectionUri;
            }

            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
            return _documentCollectionUri;
        }

        public async Task<T> GetChildResourceForCustomerAsync(Guid customerId, Guid childResourceId, string customerPropertyName, string childResourcePropertyName)
        {
            var collectionUri = GetOrCreateDocumentCollectionUri();

            var employmentProgressionForCustomerQuery = _documentClient
                ?.CreateDocumentQuery<T>(collectionUri, new FeedOptions { MaxItemCount = 1 })
                .Where(x => Guid.Parse(x.GetType().GetProperty(nameof(customerPropertyName)).GetValue(nameof(customerId)).ToString()) == customerId &&
                    Guid.Parse(x.GetType().GetProperty(nameof(childResourcePropertyName)).GetValue(nameof(childResourcePropertyName)).ToString()) == childResourceId)

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
            var collectionUri = GetOrCreateDocumentCollectionUri();

            var childrenResourcesQuery = _documentClient.CreateDocumentQuery<T>(collectionUri)
                .Where(x => Guid.Parse(x.GetType().GetProperty(nameof(customerId)).GetValue(nameof(customerId)).ToString()) == customerId).AsDocumentQuery();

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
            var collectionUri = GetOrCreateDocumentCollectionUri();
            
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

        private Uri CreateDocumentUri(Guid contactDetailsId)
        {
            return UriFactory.CreateDocumentUri(_databaseId, _collectionId, contactDetailsId.ToString());
        }

        public async Task<string> GetChildResourceForCustomerToPatchAsync(Guid customerId, Guid childResourceId, string customerPropertyName, string childResourcePropertyName)
        {
            var collectionUri = GetOrCreateDocumentCollectionUri();

            var childResourceQuery = _documentClient
                ?.CreateDocumentQuery<T>(collectionUri, new FeedOptions { MaxItemCount = 1 })
                    .Where(x => Guid.Parse(x.GetType().GetProperty(nameof(customerPropertyName)).GetValue(nameof(customerId)).ToString()) == customerId &&
                    Guid.Parse(x.GetType().GetProperty(nameof(childResourcePropertyName)).GetValue(nameof(customerId)).ToString()) == childResourceId)
                    .AsDocumentQuery();

            if (childResourceQuery == null)
            {
                return null;
            }

            var childResource = await childResourceQuery.ExecuteNextAsync();
            return childResource?.FirstOrDefault()?.ToString();
        }

        private Uri GetOrCreateDocumentCollectionUri()
        {
            if (_documentCollectionUri != null)
            {
                return _documentCollectionUri;
            }

            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
            return _documentCollectionUri;
        }
    }
}