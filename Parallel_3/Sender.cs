using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Parallel_3
{
    class Sender
    {
        public void Send(string message, string queue)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                    //string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    //var properties = channel.CreateBasicProperties();
                    //properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                                         routingKey: queue,
                                         basicProperties: null,
                                         body: body);
                    //Console.WriteLine("Sent {0} to {1}", message, queue);
                }
            }
        }
    }
}
