using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Messaging.ServiceBus;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Campground.Shared.Communication.AzureServiceBus.Interfaces;
using System.Text.Json;

namespace Campground.Shared.Communication.AzureServiceBus
{
    public class AzureServiceBusHandler : IMessageSender
    {
        private readonly ServiceBusClient _client;
        private readonly Dictionary<string, ServiceBusProcessor> _processors = new();

        public AzureServiceBusHandler(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureServiceBus")!;
            _client = new ServiceBusClient(connectionString);
        }

        public void RegisterMessageHandler(string queueName, IMessageHandler messageHandler)
        {
            var processor = _client.CreateProcessor(queueName);
            processor.ProcessMessageAsync += async args =>
            {
                string messageBody = args.Message.Body.ToString();
                Console.WriteLine($"Received message: {messageBody}");

                await messageHandler.HandleMessageAsync(messageBody);

                await args.CompleteMessageAsync(args.Message);
            };
            processor.ProcessErrorAsync += ErrorHandler;
            _processors[queueName] = processor;
        }

        public async Task StartProcessingAsync(string queueName)
        {
            if(_processors.TryGetValue(queueName, out var processor))
            {
                await processor.StartProcessingAsync();
            }
        }

        public async Task SendMessageAsync<T>(string queueName, T messageObject)
        {
            try
            {
                var message = JsonSerializer.Serialize(messageObject)!;
                ServiceBusSender sender = _client.CreateSender(queueName);
                await sender.SendMessageAsync(new ServiceBusMessage(message));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error al enviar el mensaje: {ex.Message}");
                if(ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }

        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
            return Task.CompletedTask;
        }
    }

}
