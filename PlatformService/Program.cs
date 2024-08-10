using BlogApp.Net.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data.Repos.Caching;
using PlatformService.Data.Repos;
using PlatformService.Data;
using PlatformService.Services.RabbitMq_MassTransit;
using StackExchange.Redis;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.Converters.Add(new AsyncStateMachineBoxConverter());

}); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IDriverNotificationPublisherService, DriverNotificationPublisherService>();

// Configure RabbitMQ
var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
var rabbitMqPort = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";
var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
var rabbitMqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "guest";

builder.Services.AddMassTransit(x =>
{


    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqHost,"/", h =>
        {
            h.Username(rabbitMqUser);
            h.Password(rabbitMqPassword);
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Configure Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse("redis:6379,abortConnect=false");
    return ConnectionMultiplexer.Connect(configuration);
});


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));

// Register MassTransit Hosted Service
builder.Services.AddMassTransitHostedService();


// Configure AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
PrepDb.PrepPopulation(app, builder.Environment.IsProduction());


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
