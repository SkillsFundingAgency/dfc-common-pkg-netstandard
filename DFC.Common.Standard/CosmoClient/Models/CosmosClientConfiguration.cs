namespace DFC.Common.Standard.CosmosClient.Models
{
    public class CosmosClientConfiguration
    {
        public string DatabaseId { get; set; }
        public string CollectionId { get; set; }
        public string CustomerDatabaseId { get; set; }
        public string CustomerCollectionId { get; set; }
        public string SubscriptionKey { get; set; }
        public string Address { get; set; }
        public string CustomerResourceIdName { get; set; }
        public string ChildResourceIdName { get; set; }
    }
}
