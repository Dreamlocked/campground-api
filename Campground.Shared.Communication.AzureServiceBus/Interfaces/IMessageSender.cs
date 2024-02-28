using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Shared.Communication.AzureServiceBus.Interfaces
{
    public interface IMessageSender
    {
        Task SendMessageAsync<T>(string queueName, T message);
    }
}
