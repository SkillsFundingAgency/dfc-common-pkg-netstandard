using System;

namespace DFC.Common.Standard.ServiceBusClient.Models
{
    public class MessageModel
    {
        public string TitleMessage { get; set; }
        public Guid? CustomerGuid { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string URL { get; set; }
        public bool IsNewCustomer { get; set; }
        public string TouchpointId { get; set; }

        public MessageModel()
        {
        }

        public MessageModel(Guid? customerGuid, string touchpointId , string uRL, string titleMessage, DateTime? lastModifiedDate, bool isNewCustomer)
        {
            CustomerGuid = customerGuid;
            TouchpointId = touchpointId;
            URL = uRL;
            TitleMessage = titleMessage;
            LastModifiedDate = lastModifiedDate;
            IsNewCustomer = isNewCustomer;
        }
    }
}
