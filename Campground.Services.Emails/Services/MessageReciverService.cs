using Azure.Messaging.ServiceBus;
using Campground.Services.Emails.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace Campground.Services.Emails.Services
{
    public class MessageReceiverService
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private readonly EmailService _emailService;

        public MessageReceiverService(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
            _client = new ServiceBusClient(Environment.GetEnvironmentVariable("AzureServiceBus") ?? _configuration!.GetConnectionString("AzureServiceBus"), new ServiceBusClientOptions()
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            });
            _processor = _client.CreateProcessor(_configuration["Queues:Email"]!);
        }

        public async Task RegisterMessageHandlerAsync()
        {
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync();
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string messageBody = args.Message.Body.ToString();
            Console.WriteLine($"Received message: {messageBody}");

            // Aquí puedes agregar la lógica para ejecutar una acción cuando recibes un mensaje
            var email = JsonSerializer.Deserialize<Email>(messageBody)!;

            _emailService.SendEmail(email);

            // Completa el mensaje
            await args.CompleteMessageAsync(args.Message);
        }


        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Message handler encountered an exception {args.Exception}.");
            return Task.CompletedTask;
        }
    }

}
