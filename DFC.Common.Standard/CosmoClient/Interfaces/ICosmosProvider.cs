using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace NCS.DSS.CosmosDocumentClient.Interfaces
{
    public interface ICosmosProvider<T> where T : class, new()
    {
        Task<ResourceResponse<Document>> CreateResourceAsync(T resource);
        T GetResource(SqlQuerySpec sqlQuerySpec);  
        List<T> GetResources(SqlQuerySpec sqlQuerySpec);
        Task<ResourceResponse<Document>> UpdateResourceAsync(string jsonString, string resourceId);
    }
}