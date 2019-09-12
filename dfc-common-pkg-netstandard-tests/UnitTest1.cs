using DFC.Common.Standard.CosmoClient.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NCS.DSS.CosmosDocumentClient.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void GetResourceSingleCondition()
        {
            // arrange
            var cosmosProviderConfig = new CosmosProviderConfiguration
            {
                CollectionId = "employmentprogressions",
                DatabaseId = "employmentprogressions",
                CustomerCollectionId = "customers",
                CustomerDatabaseId = "customers",
                Address = "https://dss-at-shared-cdb.documents.azure.com:443/",
                SubscriptionKey = "gN8fmksIp9YAz7a3GHARnMlr7EdA3MWg3vEVEHkIrWfqHhl9Mpxd651fwmgqfCEgmuFEaGDU3OJLNouL6zlbmQ==",
                ChildResourceIdName = "id",
                CustomerResourceIdName = "CustomerId"
            };

            var customerId = "ca8757b6-54f5-4c4c-b1a2-403c9eb01877";

            var query = new SqlQuerySpec()
            {
                QueryText = $"SELECT * FROM {cosmosProviderConfig.CollectionId} f WHERE (f.{cosmosProviderConfig.CustomerResourceIdName} = @customerId)",

                Parameters = new SqlParameterCollection()
                {
                    new SqlParameter("@customerId", customerId)
                }
            };

            var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

            // act
            var document = provider.GetResource(query);

            // assert
            Assert.True(document.CustomerId.HasValue);
            Assert.True(document.CustomerId.Value .ToString() == customerId);
        }

        [Fact]
        public void GetResourceMultipleConditions()
        {
            // arrange
            var cosmosProviderConfig = new CosmosProviderConfiguration
            {
                CollectionId = "employmentprogressions",
                DatabaseId = "employmentprogressions",
                CustomerCollectionId = "customers",
                CustomerDatabaseId = "customers",
                Address = "https://dss-at-shared-cdb.documents.azure.com:443/",
                SubscriptionKey = "gN8fmksIp9YAz7a3GHARnMlr7EdA3MWg3vEVEHkIrWfqHhl9Mpxd651fwmgqfCEgmuFEaGDU3OJLNouL6zlbmQ==",
            };

            var customerId = "ca8757b6-54f5-4c4c-b1a2-403c9eb01877";
            var employmentProgressionId = "c4c26aa5-cdb1-431e-adb2-f4bcef5a301f";

            var query = new SqlQuerySpec()
            {
                QueryText = $"SELECT * FROM {cosmosProviderConfig.CollectionId} f WHERE (f.CustomerId = @customerId) AND " +
                $"(f.id = @employmentId)",

                Parameters = new SqlParameterCollection()
                {
                    new SqlParameter("@customerId", customerId),
                    new SqlParameter("@employmentId", employmentProgressionId)
                }
            };

            var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

            // act
            var document = provider.GetResource(query);

            // assert
            Assert.True(document.CustomerId.HasValue && document.EmploymentProgressionId.HasValue);
            Assert.True(document.CustomerId.Value.ToString() == customerId && document.EmploymentProgressionId.Value.ToString() == employmentProgressionId);
        }

        [Fact]
        public void GetResourcesSingleCondition()
        {
            // arrange
            var cosmosProviderConfig = new CosmosProviderConfiguration
            {
                CollectionId = "employmentprogressions",
                DatabaseId = "employmentprogressions",
                CustomerCollectionId = "customers",
                CustomerDatabaseId = "customers",
                Address = "https://dss-at-shared-cdb.documents.azure.com:443/",
                SubscriptionKey = "gN8fmksIp9YAz7a3GHARnMlr7EdA3MWg3vEVEHkIrWfqHhl9Mpxd651fwmgqfCEgmuFEaGDU3OJLNouL6zlbmQ==",
            };

            var customerId = "ca8757b6-54f5-4c4c-b1a2-403c9eb01877";
        
            var query = new SqlQuerySpec()
            {
                QueryText = $"SELECT * FROM {cosmosProviderConfig.CollectionId} f WHERE (f.CustomerId = @customerId)",

                Parameters = new SqlParameterCollection()
                {
                    new SqlParameter("@customerId", customerId)
                }
            };

            var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

            // act
            var document = provider.GetResources(query);

            // assert
            Assert.True(document.Count >= 1);
        }

        [Fact]
        public void GetResourcesMultipleConditions()
        {
            // arrange
            var cosmosProviderConfig = new CosmosProviderConfiguration
            {
                CollectionId = "employmentprogressions",
                DatabaseId = "employmentprogressions",
                CustomerCollectionId = "customers",
                CustomerDatabaseId = "customers",
                Address = "https://dss-at-shared-cdb.documents.azure.com:443/",
                SubscriptionKey = "gN8fmksIp9YAz7a3GHARnMlr7EdA3MWg3vEVEHkIrWfqHhl9Mpxd651fwmgqfCEgmuFEaGDU3OJLNouL6zlbmQ==",
            };

            var customerId = "ca8757b6-54f5-4c4c-b1a2-403c9eb01877";
            var employmentProgressionId = "c4c26aa5-cdb1-431e-adb2-f4bcef5a301f";

            var query = new SqlQuerySpec()
            {
                QueryText = $"SELECT * FROM {cosmosProviderConfig.CollectionId} f WHERE (f.CustomerId = @customerId) AND " +
                $"(f.id = @employmentId)",

                Parameters = new SqlParameterCollection()
                {
                    new SqlParameter("@customerId", customerId),
                    new SqlParameter("@employmentId", employmentProgressionId)
                }
            };

            var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

            // act
            var document = provider.GetResources(query);

            // assert
            Assert.True(document.Count >= 1);
        }

        [Fact]
        public async Task UpdateResourceAsync()
        {
            // arrange
            var cosmosProviderConfig = new CosmosProviderConfiguration
            {
                CollectionId = "employmentprogressions",
                DatabaseId = "employmentprogressions",
                CustomerCollectionId = "customers",
                CustomerDatabaseId = "customers",
                Address = "https://dss-at-shared-cdb.documents.azure.com:443/",
                SubscriptionKey = "gN8fmksIp9YAz7a3GHARnMlr7EdA3MWg3vEVEHkIrWfqHhl9Mpxd651fwmgqfCEgmuFEaGDU3OJLNouL6zlbmQ==",
            };
            var employmentProgressionId = "c4c26aa5-cdb1-431e-adb2-f4bcef5a301f";

            var query = new SqlQuerySpec()
            {
                QueryText = $"SELECT * FROM EmploymentProgressions f WHERE (f.id = @id)",

                Parameters = new SqlParameterCollection()
                {
                    new SqlParameter("@id", employmentProgressionId)
                }
            };

            var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

            // act
            var originalDocument = provider.GetResource(query);

            // patch the original document
            originalDocument.EmployerName = "Kevin Brandon";
            var jsonString = JsonConvert.SerializeObject(originalDocument);            
   
            // act
            var updatedDocument = await provider.UpdateResourceAsync(jsonString, employmentProgressionId);

            // assert
            Assert.True(updatedDocument != null);
        }


        [Fact]
        public void CreateResoruceAsync()
        {
            // arrange
            var cosmosProviderConfig = new CosmosProviderConfiguration
            {
                CollectionId = "employmentprogressions",
                DatabaseId = "employmentprogressions",
                CustomerCollectionId = "customers",
                CustomerDatabaseId = "customers",
                Address = "https://dss-at-shared-cdb.documents.azure.com:443/",
                SubscriptionKey = "gN8fmksIp9YAz7a3GHARnMlr7EdA3MWg3vEVEHkIrWfqHhl9Mpxd651fwmgqfCEgmuFEaGDU3OJLNouL6zlbmQ==",
            };

            var customerId = "ca8757b6-54f5-4c4c-b1a2-403c9eb01877";

            var query = new SqlQuerySpec()
            {
                QueryText = $"SELECT * FROM {cosmosProviderConfig.CollectionId} f WHERE (f.CustomerId = @customerId)",

                Parameters = new SqlParameterCollection()
                {
                    new SqlParameter("@customerId", customerId)
                }
            };

            var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

            // act
            var document = provider.GetResources(query);

            // assert
            Assert.True(document.Count >= 1);
        }

        //[Fact]
        //public void GetChildrenResourceForCustomer()
        //{
        //    // arrange
        //    var cosmosProviderConfig = new CosmosProviderConfiguration
        //    {
        //        CollectionId = "employmentprogressions",
        //        DatabaseId = "employmentprogressions",
        //        CustomerCollectionId = "customers",
        //        CustomerDatabaseId = "customers",
        //        Address = "https://dss-at-shared-cdb.documents.azure.com:443/",
        //        SubscriptionKey = "gN8fmksIp9YAz7a3GHARnMlr7EdA3MWg3vEVEHkIrWfqHhl9Mpxd651fwmgqfCEgmuFEaGDU3OJLNouL6zlbmQ==",
        //        ChildResourceIdName = "id",
        //        CustomerResourceIdName = "CustomerId"
        //    };

        //    var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

        //    // act
        //    var customerResource = provider.GetResources();

        //    // assert
        //    Assert.True(customerResource != null);
        //}


        //[Fact]
        //public void GetChildResourceForCustomer()
        //{
        //    // arrange
        //    var cosmosProviderConfig = new CosmosProviderConfiguration
        //    {
        //        CollectionId = "employmentprogressions",
        //        DatabaseId = "employmentprogressions",
        //        CustomerCollectionId = "customers",
        //        CustomerDatabaseId = "customers",
        //        Address = "https://dss-at-shared-cdb.documents.azure.com:443/",
        //        SubscriptionKey = "gN8fmksIp9YAz7a3GHARnMlr7EdA3MWg3vEVEHkIrWfqHhl9Mpxd651fwmgqfCEgmuFEaGDU3OJLNouL6zlbmQ==",
        //        ChildResourceIdName = "id",
        //        CustomerResourceIdName = "CustomerId"
        //    };

        //    var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

        //    // act
        //    var customerResource = provider.GetChildResourceForCustomer(Guid.Parse("ca8757b6-54f5-4c4c-b1a2-403c9eb01877"), Guid.Parse("c4c26aa5-cdb1-431e-adb2-f4bcef5a301f"));

        //    // assert
        //    Assert.True(customerResource != null);
        //}


        //[Fact]
        //public void GetChildResourceToPatchForCustomer()
        //{
        //    // arrange
        //    var cosmosProviderConfig = new CosmosProviderConfiguration
        //    {
        //        CollectionId = "employmentprogressions",
        //        DatabaseId = "employmentprogressions",
        //        CustomerCollectionId = "customers",
        //        CustomerDatabaseId = "customers",
        //        Address = "https://dss-at-shared-cdb.documents.azure.com:443/",
        //        SubscriptionKey = "gN8fmksIp9YAz7a3GHARnMlr7EdA3MWg3vEVEHkIrWfqHhl9Mpxd651fwmgqfCEgmuFEaGDU3OJLNouL6zlbmQ==",
        //        ChildResourceIdName = "id",
        //        CustomerResourceIdName = "CustomerId"
        //    };

        //    var query = new SqlQuerySpec()
        //    {
        //        QueryText = $"SELECT * FROM {cosmosProviderConfig.CollectionId} f WHERE (f.{cosmosProviderConfig.CustomerResourceIdName} = @customerId)",

        //        Parameters = new SqlParameterCollection()
        //                {
        //                    new SqlParameter("@customerId", "ca8757b6-54f5-4c4c-b1a2-403c9eb01877")
        //                }
        //    };

        //    var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

        //    // act
        //    var customerResource = provider.GetResource(query);

        //    // assert
        //    Assert.True(customerResource != null);
        //}


        //[Fact]
        //public async Task DoesCustomerHaveTerminationDate()
        //{
        //    // arrange
        //    var cosmosProviderConfig = new CosmosProviderConfiguration
        //    {
        //        CollectionId = "employmentprogressions",
        //        DatabaseId = "employmentprogressions",
        //        CustomerCollectionId = "customers",
        //        CustomerDatabaseId = "customers",
        //        Address = "https://dss-at-shared-cdb.documents.azure.com:443/",
        //        SubscriptionKey = "gN8fmksIp9YAz7a3GHARnMlr7EdA3MWg3vEVEHkIrWfqHhl9Mpxd651fwmgqfCEgmuFEaGDU3OJLNouL6zlbmQ==",
        //        ChildResourceIdName = "id",
        //        CustomerResourceIdName = "CustomerId"
        //    };


        //    // multiple
        //    //    var query = _documentClient.CreateDocumentQuery<T>(collectionUri, new SqlQuerySpec()
        //    //    {
        //    //        QueryText = $"SELECT * FROM {_cosmosProviderConfiguration.CollectionId} f " +
        //    //$"WHERE (f.{_cosmosProviderConfiguration.CustomerResourceIdName} = @customerId AND " +
        //    //$"f.{_cosmosProviderConfiguration.ChildResourceIdName} = @ChildResourceId)",

        //    //        Parameters = new SqlParameterCollection()
        //    //            {
        //    //                new SqlParameter("@customerId", customerId.ToString()),
        //    //                new SqlParameter("@ChildResourceId", childResourceId.ToString())
        //    //            }
        //    //    });






        //    //try
        //    //{
        //    //    var response = await _documentClient.ReadDocumentAsync(documentUri);
        //    //    var dateOfTermination = response.Resource?.GetPropertyValue<DateTime?>("DateOfTermination");

        //    //    return dateOfTermination.HasValue;
        //    //}
        //    //catch (DocumentClientException)
        //    //{
        //    //    return false;
        //    //}


        //    //public bool DoesChildResourceExistForCustomer(Guid customerId, string customerPropertyName)
        //    //{
        //    //    var employmentProgressionForCustomerQuery = _documentClient.CreateDocumentQuery<T>(_collectionUri, new FeedOptions { MaxItemCount = 1 });
        //    //    var result = employmentProgressionForCustomerQuery.Where(x => Guid.Parse(x.GetType().GetProperty(nameof(customerPropertyName)).GetValue(nameof(customerPropertyName)).ToString()) == customerId).AsEnumerable().Any();

        //    //    return result;
        //    //}



        //    var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

        //    // act
        //    var hasTerminationDate = await provider.GetResource("ca8757b6-54f5-4c4c-b1a2-403c9eb01877");

        //    // assert
        //    Assert.False(hasTerminationDate);
        //}
    }
}
