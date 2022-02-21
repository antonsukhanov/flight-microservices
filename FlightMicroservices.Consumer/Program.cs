using Confluent.Kafka;
using Confluent.Kafka.DependencyInjection;
using FlightMicroservices.Consumer;
using FlightMicroservices.Consumer.Models.Contexts;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql("Host=localhost;Port=5432;Database=flight-consumer-db;Username=consumer;Password=consumer"));
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