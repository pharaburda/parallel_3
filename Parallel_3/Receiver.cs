using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace Parallel_3
{
    class Receiver
    {
        public string Receive(string queue)
        {
            string message = "";
            int number = 0;
            string type = "";
            int count = 0;
            var split = queue.Split('_');
            type = split[0];
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
                        Console.WriteLine(" Musician {0} received {1}", queue, message);
                        
                        switch (type)
                        {
                            case "1":
                                if (Int32.Parse(message) > number) number = Int32.Parse(message);
                                break;
                            case "2":
                                count++;
                                break;
                        }
                        
                    };

                    if (message != "") return type == "1" ? number.ToString() : count.ToString();

                    channel.BasicConsume(queue: queue,
                                         autoAck: true,
                                         consumer: consumer);

                    //Console.ReadLine();
                    Thread.Sleep(3000);
                }                
            }
        }
    }
}
