using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;

namespace OrderConfirmationService
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ConsumerConfig consumerConfig = new ConsumerConfig
            {
                BootstrapServers = $"localhost:{Helper.GetKafkaBrokerPort(Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.Parent?.FullName!)}",
                GroupId = "orer-consumer",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(
                        new ConsumerBuilder<string, string>(consumerConfig).Build());
                    services.AddHostedService<ConsumerService>();
                });
            var host = builder.Build();
            var kafkaConsumerService = host.Services.GetRequiredService<IHostedService>();
            await kafkaConsumerService.StartAsync(default);
            await Task.Delay(Timeout.Infinite);
        }
    }
}
