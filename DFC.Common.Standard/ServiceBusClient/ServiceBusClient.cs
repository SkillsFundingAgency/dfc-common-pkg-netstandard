using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DFC.Common.Standard.ServiceBusClient.Models;
using DFC.Common.Standard.ServiceBusClient.Interfaces;
using Microsoft.Azure.ServiceBus;

namespace DFC.Common.Standard.ServiceBusCleint
{
    public class ServiceBusClient : IServiceBusClient
    {
        private readonly ServiceBusConfiguration _serviceBusConfiguration;
        private readonly QueueClient _queueClient;

        public ServiceBusClient(ServiceBusConfiguration serviceBusConfiguration)
        {
            _serviceBusConfiguration = serviceBusConfiguration;
            _queueClient = new QueueClient(_serviceBusConfiguration.ConnectionString, _serviceBusConfiguration.QueueName);
        }

        public async Task SendMessageAsync(MessageModel messageModel, string reqUrl)
        {
            var serviceBussMessage = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageModel)))
            {
                ContentType = "application/json",
                MessageId = $"{messageModel.CustomerGuid} {DateTime.UtcNow}"
            };

            await _queueClient.SendAsync(serviceBussMessage);
        }
    }
}
