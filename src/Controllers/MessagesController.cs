using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Domains;
using System.Text.Json;
using System.Text;

namespace RabbitMQ.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ConnectionFactory _factory;
        private readonly RabbitMqConfiguration _configuration;

        public MessagesController(IOptions<RabbitMqConfiguration> option)
        {
            _configuration = option.Value;
            _factory = new ConnectionFactory()
            {
                HostName = _configuration.Host
            };
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] Message message)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: _configuration.Queue,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    
                    var stringfiedMessage = JsonSerializer.Serialize(message);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: _configuration.Queue,
                        basicProperties: null,
                        body: bytesMessage);

                    Console.WriteLine("Message send to queue: " + stringfiedMessage);
                }
            }

            return Accepted();
        }
    }
}
