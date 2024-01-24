using Azure.Messaging.ServiceBus;
using ms_correo.Models;
using Newtonsoft.Json;

namespace campground_api.Services
{
    public class BookingService(MessageSenderService messageSenderService)
    {
        private readonly MessageSenderService _messageSenderService = messageSenderService;


    }
}
