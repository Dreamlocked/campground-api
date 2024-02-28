using Campground.Shared.Communication.AzureServiceBus.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Shared.Communication.AzureServiceBus
{
    public static class CustomMiddlewareExtension
    {
        public static IServiceCollection AddAzureServiceBusHandler(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<AzureServiceBusHandler>();
            services.AddSingleton<IMessageSender, AzureServiceBusHandler>();
            return services;
        }
    }

}
