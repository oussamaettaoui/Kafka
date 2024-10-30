using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared;

namespace OrderConfirmationService
{
    public class ConsumerService : IHostedService
    {
        private readonly IConsumer<string, string> _consumer;
        public ConsumerService(IConsumer<string, string> consumer) => _consumer = consumer;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe("order-events");
            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    if (consumeResult is null)
                    {
                        return;
                    }
                    var orderDetails = JsonConvert.DeserializeObject<OrderDetails>(consumeResult.Message.Value);
                    Console.WriteLine($"Recieved message: " + $"Order Id : {orderDetails.OrderId}, Product name: {orderDetails.ProductName}, Price: {orderDetails.Price}, Order date: {orderDetails.OrderDate}");
                }
            }, cancellationToken);
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
