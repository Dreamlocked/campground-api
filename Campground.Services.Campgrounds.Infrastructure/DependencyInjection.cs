using Azure.Storage.Blobs;
using Campground.Services.Campgrounds.Infrastructure.Data;
using Campground.Services.Campgrounds.Infrastructure.Data.Repository;
using Campground.Services.Campgrounds.Infrastructure.Data.Unit_of_Work;
using Campground.Services.Campgrounds.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Services.Campgrounds.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<CampgroundContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DbContext") ?? 
                configuration.GetConnectionString("DbContext")));

            services.AddScoped(x => new BlobServiceClient(Environment.GetEnvironmentVariable("BlobStorage") ?? 
                configuration.GetConnectionString("BlobStorage")));

            // Add services to the container.
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<CampgroundRepository>();

            return services;
        }
    }
}
