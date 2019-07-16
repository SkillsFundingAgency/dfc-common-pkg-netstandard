using Microsoft.Azure.Documents;

namespace NCS.DSS.ActionPlan.Cosmos.Client
{
    public interface ICosmosDocumentClient
    {
        IDocumentClient GetDocumentClient();
    }
}