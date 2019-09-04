namespace DFC.Common.Standard.ServiceBusClient.Models
{
    public class ServiceBusConfiguration
    {
        public string ConnectionString { get; internal set; }
        public string QueueName { get; internal set; }
    }
}