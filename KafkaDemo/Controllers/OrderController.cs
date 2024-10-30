using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;

namespace KafkaDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IProducer<string,string> _producer;
        private const string Topic = "order-events";
        public OrderController(IProducer<string,string> producer)
        {
            _producer = producer;
        }
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(OrderDetails orderDetails)
        {
            try
            {
                var kafkaMassage = new Message<string, string>
                {
                    Value = JsonConvert.SerializeObject(orderDetails)
                };
                await _producer.ProduceAsync(Topic, kafkaMassage);
                return Ok("Order placed successfully");
            }
            catch (ProduceException<string,string> ex)
            {
                return BadRequest($"Error publishing message : {ex.Error.Reason}");
            }
        }
    }
}
