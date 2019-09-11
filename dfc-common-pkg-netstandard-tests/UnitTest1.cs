using DFC.Common.Standard.CosmoClient.Models;
using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NCS.DSS.CosmosDocumentClient.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task DoesCustomerResourceExistAsync()
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

            var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

            // act
            var doesCustomerExist = await provider.DoesCustomerResourceExistAsync(new Guid("ca8757b6-54f5-4c4c-b1a2-403c9eb01877"));

            // assert
            Assert.True(doesCustomerExist);
        }

        [Fact]
        public void GetChildrenResourceForCustomer()
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

            var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

            // act
            var customerResource = provider.GetChildrenResourceForCustomer(Guid.Parse("ca8757b6-54f5-4c4c-b1a2-403c9eb01877"));
            
            // assert
            Assert.True(customerResource != null);
        }


        [Fact]
        public void GetChildResourceForCustomer()
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

            var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

            // act
            var customerResource = provider.GetChildResourceForCustomer(Guid.Parse("ca8757b6-54f5-4c4c-b1a2-403c9eb01877"), Guid.Parse("c4c26aa5-cdb1-431e-adb2-f4bcef5a301f"));

            // assert
            Assert.True(customerResource != null);
        }


        [Fact]
        public void GetChildResourceToPatchForCustomer()
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

            var provider = new CosmosProvider<EmploymentProgression>(new DocumentClient(new Uri(cosmosProviderConfig.Address), cosmosProviderConfig.SubscriptionKey), cosmosProviderConfig);

            // act
            var customerResource = provider.GetChildResourceToPatchForCustomer(Guid.Parse("ca8757b6-54f5-4c4c-b1a2-403c9eb01877"), Guid.Parse("c4c26aa5-cdb1-431e-adb2-f4bcef5a301f"));

            // assert
            Assert.True(customerResource != null);
        }        
    }
}
