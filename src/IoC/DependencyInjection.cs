﻿using Application.Contracts.Settings;
using Application.EventHandlers;
using Application.Interfaces;
using Crosscutting.Services;
using Data.Context;
using Data.Interfaces.MongoDb;
using Data.Interfaces.PostgreDb;
using Data.Repositories.PostgreDb;
using Data.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using System.Text.Json.Serialization;

namespace IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PostgreDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSql"))
            );

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddScoped(typeof(IMongoDbContext<>), typeof(Data.Repositories.MongoDb.GenericRepository<>));
            var PostgreContext = services.BuildServiceProvider().GetRequiredService<PostgreDbContext>();

            Initializer.Initialize(PostgreContext);
            return services;

        }

        public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbSettings = new MongoDbSettings();
            new ConfigureFromConfigurationOptions<MongoDbSettings>(
                configuration.GetSection("MongoDbSettings"))
                    .Configure(mongoDbSettings);

            services.AddSingleton(mongoDbSettings);

            var kafkaSettings = new KafkaSettings();
            new ConfigureFromConfigurationOptions<KafkaSettings>(
                configuration.GetSection("KafkaSettings"))
                    .Configure(kafkaSettings);

            services.AddSingleton(kafkaSettings);


            return services;
        }
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            return services;
        }

        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("Application")));
            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
            services.AddTransient<IEventBusService, EventBusService>();
            services.AddHostedService<BackgroundWorkerService>();
            return services;
        }
        public static IServiceCollection AddHandler(this IServiceCollection services)
        {
            services.AddTransient<VehicleCreatedEventHandler>();
            return services;
        }

        public static IServiceCollection AddWebApiConfiguration(this IServiceCollection services)
        {
            services.AddSession();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            return services;

        }

        public static WebApplicationBuilder LogBuilder(this WebApplicationBuilder webApplication)
        {

            Log.Logger = new LoggerConfiguration()
                             .MinimumLevel.Information()
                             .Enrich.FromLogContext()
                             .WriteTo.Console()
                             .CreateLogger();

            webApplication.Host.UseSerilog();
            return webApplication;
        }
    }
}
