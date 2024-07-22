using Application.Contracts.Settings;
using Application.EventHandlers;
using Application.Interfaces;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Core.BackgroundWorker;
using Crosscutting.Services;
using Data.Context;
using Data.Interfaces.MongoDb;
using Data.Interfaces.PostgreDb;
using Data.Repositories.PostgreDb;
using Data.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;

namespace IoC
{
    public static class DependencyInjection
    {
        private const string CorsName = "CORSPOLICY";

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

        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddMvc();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            return services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "API Delivery Dispatch",
                        Version = GetVersion(),
                        Description = string.Format("Environment: {0}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")),
                    });
            });
        }

        private static string GetVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.ProductVersion!;
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
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, string origins)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy(CorsName, builder => builder.WithOrigins(origins.Split(';'))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders(new List<string>() { "X-Pagination", "X-Summary", "Content-Disposition" }.ToArray()));
            });
        }

        public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app)
        {
            return app.UseCors(CorsName);
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env != null && env.EnvironmentName == "Production")
                return app;

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "delivery-dispatch/swagger/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    string serverUrl = $"https://{httpReq.Host.Host}";
                    if (httpReq.Host.Port != null)
                        serverUrl += $":{httpReq.Host.Port}";
                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "delivery-dispatch/swagger";
                c.SwaggerEndpoint($"/delivery-dispatch/swagger/{app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions.OrderByDescending(p => p.GroupName).Select(description => description.GroupName).FirstOrDefault()}/swagger.json",
                    $"API Delivery Dispatch {app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions.OrderByDescending(p => p.GroupName).Select(description => description.GroupName).FirstOrDefault()?.ToUpperInvariant()}");
            });

            return app;
        }
    }
}
