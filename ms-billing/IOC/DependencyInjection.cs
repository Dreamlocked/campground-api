using Campground.Shared.Communication.AzureServiceBus;
using Campground.Shared.Communication.AzureServiceBus.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ms_billing.Data;
using ms_billing.Data.Repository;
using ms_billing.HandlerMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ms_billing.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection Infrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BillingStoreDatabaseSettings>(
                configuration.GetSection("BillingStoreDatabase"));

            services.AddScoped<BillingsRepository>();
            services.AddAzureServiceBusHandler(configuration);
            services.AddSingleton<MessageSender>();

            return services;
        }
    }
}
