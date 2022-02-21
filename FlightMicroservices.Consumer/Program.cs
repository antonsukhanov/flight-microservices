using Confluent.Kafka;
using Confluent.Kafka.DependencyInjection;
using FlightMicroservices.Consumer;
using FlightMicroservices.Consumer.Models.Contexts;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(context.Configuration.GetConnectionString("ApplicationDatabase")));
        services.AddKafkaClient(new ConsumerConfig
        {
            BootstrapServers = "kafka:9093",
            GroupId = "flight-microservices-consumers",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            AllowAutoCreateTopics = true
        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();