using DFC.Common.Standard.ServiceBusClient.Models;
using System.Threading.Tasks;

namespace DFC.Common.Standard.ServiceBusClient.Interfaces
{
    public interface IServiceBusClient
    {
        Task SendMessageAsync(MessageModel messageModel, string reqUrl);
    }
}