
using campground_api.Models;
using campground_api.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Configuration;
using System.Text;

namespace campground_api.IOC
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("Custom", o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JwtKey") ?? builder.Configuration["Jwt:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };

                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[builder.Configuration["Jwt:CookieName"]!];
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Custom")
                    .Build();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Campground",
                    Version = "v1.0.1"
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("https://campgrounds-frontend.vercel.app")
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .AllowAnyMethod();
                    });
            });

            builder.Services.AddDbContext<CampgroundContext>(
                options =>
                   options.UseSqlServer(Environment.GetEnvironmentVariable("DbContext") ?? builder.Configuration.GetConnectionString("DbContext")));

            builder.Services.AddAzureClients(azureBuilder =>
            {
                azureBuilder.AddBlobServiceClient(Environment.GetEnvironmentVariable("Storage") ?? builder.Configuration.GetConnectionString("Storage")!);
            });

            builder.Services.AddSignalR();
            builder.Services.AddHealthChecks();

            builder.Services.AddScoped<MessageSenderService>();
            builder.Services.AddScoped<CampgroundService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ReviewService>();
            builder.Services.AddScoped<BookingService>();
            builder.Services.AddScoped<NotificationService>();

            builder.Services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
    }
}
