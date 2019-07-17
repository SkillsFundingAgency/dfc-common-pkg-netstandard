using Microsoft.Azure.Documents;

namespace DFC.Common.Standard.CosmosDocumentClient
{
    public interface ICosmosDocumentClient
    {
        IDocumentClient GetDocumentClient();
    }
}