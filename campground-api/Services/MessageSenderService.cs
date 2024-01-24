using Azure.Messaging.ServiceBus;
using ms_correo.Models;
using Newtonsoft.Json;
using System.Configuration;

namespace campground_api.Services
{
    public class MessageSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        public MessageSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new ServiceBusClient(Environment.GetEnvironmentVariable("AzureServiceBus") ?? _configuration!.GetConnectionString("AzureServiceBus"), new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });
            _sender = _client.CreateSender(_configuration["Queues:Email"]!);
        }

        private async Task<bool> SendMessagesAsync(ServiceBusMessageBatch messageBatch)
        {
            try
            {
                await _sender.SendMessagesAsync(messageBatch);
                return true;
            }
            finally
            {
                await _sender.DisposeAsync();
                await _client.DisposeAsync();
            }
        }

        private async Task<ServiceBusMessageBatch> CreateMessageBatchAsync()
        {
            return await _sender.CreateMessageBatchAsync();
        }

        public async Task<bool> SendMessage<T>(T messageObject)
        {
            try
            {
                ServiceBusMessageBatch messageBatch = await CreateMessageBatchAsync();

                string emailString = JsonConvert.SerializeObject(messageObject);

                ServiceBusMessage message = new ServiceBusMessage(emailString);

                if(!messageBatch.TryAddMessage(message))
                {
                    throw new Exception("El mensaje es demasiado grande para caber en el lote.");
                }

                return await SendMessagesAsync(messageBatch);
            }
            catch(Exception)
            {

                throw;
            }
        }

    }
}
