using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.Common.Standard.CosmoClient.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json.Linq;

namespace NCS.DSS.CosmosDocumentClient
{
    public class CosmosProvider<T> where T : class, new()
    {
        private readonly IDocumentClient _documentClient;
        private readonly CosmosProviderConfiguration _cosmosProviderConfiguration;
        private Uri _documentCollectionUri;

        public CosmosProvider(IDocumentClient documentClient, CosmosProviderConfiguration cosmosProviderConfiguration)
        {
            _documentClient = documentClient;
            _cosmosProviderConfiguration = cosmosProviderConfiguration;
        }

        public async Task<bool> DoesCustomerResourceExistAsync(Guid customerId)
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
            return UriFactory.CreateDocumentUri(_cosmosProviderConfiguration.CustomerDatabaseId, _cosmosProviderConfiguration.CustomerCollectionId, customerId.ToString());
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

            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_cosmosProviderConfiguration.DatabaseId, _cosmosProviderConfiguration.CollectionId);
            return _documentCollectionUri;
        }

        public T GetChildResourceForCustomer(Guid customerId, Guid childResourceId)
        {
            var collectionUri = GetOrCreateDocumentCollectionUri();

            var query = _documentClient.CreateDocumentQuery<T>(collectionUri, new SqlQuerySpec()
            {
                QueryText = $"SELECT * FROM {_cosmosProviderConfiguration.CollectionId} f " +
                    $"WHERE (f.{_cosmosProviderConfiguration.CustomerResourceIdName} = @customerId AND " +
                    $"f.{_cosmosProviderConfiguration.ChildResourceIdName} = @ChildResourceId)",

                Parameters = new SqlParameterCollection()
                    {
                        new SqlParameter("@customerId", customerId.ToString()),
                        new SqlParameter("@ChildResourceId", childResourceId.ToString())
                    }
            });

            return query.ToList().FirstOrDefault();
        }

        public string GetChildResourceToPatchForCustomer(Guid customerId, Guid childResourceId)
        {
            var childResource = GetChildResourceForCustomer(customerId, childResourceId);

            if (childResource == null)
            {
                return string.Empty;
            }

            return childResource.ToString();
        }

        public List<T> GetChildrenResourceForCustomer(Guid customerId)
        {
            var collectionUri = GetOrCreateDocumentCollectionUri();

            var query = _documentClient.CreateDocumentQuery<T>(collectionUri, new SqlQuerySpec()
            {
                QueryText = $"SELECT * FROM {_cosmosProviderConfiguration.CollectionId} f WHERE (f.{_cosmosProviderConfiguration.CustomerResourceIdName} = @customerId)",

                Parameters = new SqlParameterCollection()
                    {
                        new SqlParameter("@customerId", customerId.ToString())
                    }
            });

            return query.ToList();
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
            return UriFactory.CreateDocumentUri(_cosmosProviderConfiguration.DatabaseId, _cosmosProviderConfiguration.CollectionId, contactDetailsId.ToString());
        }

        private Uri GetOrCreateDocumentCollectionUri()
        {
            if (_documentCollectionUri != null)
            {
                return _documentCollectionUri;
            }

            _documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_cosmosProviderConfiguration.DatabaseId, _cosmosProviderConfiguration.CollectionId);
            return _documentCollectionUri;
        }
    }
}