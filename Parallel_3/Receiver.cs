using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Parallel_3
{
    class Receiver
    {
        public int Receive(string queue)
        {
            string message = "";
            int number = 0;
            while (true)
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queue,
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        message = Encoding.UTF8.GetString(body);                        
                        Console.WriteLine("Received {0} from {1}", message, queue);
                        if (Int32.Parse(message) > number) number = Int32.Parse(message);

                    };

                    Console.WriteLine("Message {0}", number);
                    if (message != "") return number;

                    channel.BasicConsume(queue: queue,
                                         autoAck: true,
                                         consumer: consumer);
                    
                    Console.ReadLine();                    
                }                
            }
        }
    }
}
