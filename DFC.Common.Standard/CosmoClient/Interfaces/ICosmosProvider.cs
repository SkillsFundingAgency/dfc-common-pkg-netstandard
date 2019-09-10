using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NCS.DSS.CosmosDocumentClient.Interfaces
{
    public interface ICosmosProvider<T> where T : class
    {
        Task<bool> DoesCustomerResourceExist(Guid customerId);
        Task<bool> DoesCustomerHaveTerminationDate(Guid customerId);
        Task<T> GetChildResourceForCustomerAsync(Guid customerId, Guid childResourceId);
        Task<List<T>> GetChildResourceForCustomerAsync(Guid customerId);
        Task<ResourceResponse<Document>> CreateChildResourceAsync(T childResource);
        Task<ResourceResponse<Document>> UpdateResourceAsync(string childObjectAsJson, Guid childResourceId);
        Task<string> GetChildResourceForCustomerToPatchAsync(Guid customerId, Guid childResourceId);
    }
}