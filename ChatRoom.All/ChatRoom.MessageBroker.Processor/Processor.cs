using ChatRoom.Common;
using ChatRoom.Common.QueueChatRoom;
using ChatRoom.MessageBroker.Processor.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;
using System.Text;

namespace ChatRoom.MessageBroker.Processor
{
    public class Processor : IProcessor
    {
        Dictionary<string, IExternalApiHandler> externalApiHandler = new Dictionary<string, IExternalApiHandler>()
        {
            { "/stock=APPL", new CsvExternalApi() },
            { "/day_range=APPL", new XmlExternalApi() }
        };

        public void ProcessMessage()
        {
            var cola = WebConfig.RabbitMQ.QueueName;
            var factory = new ConnectionFactory() { HostName = WebConfig.RabbitMQ.HostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(cola, false, false, false, null);
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(cola, true, consumer);

                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        var message = Encoding.UTF8.GetString(ea.Body);
                        var queueMessage = JsonSerializerHelper.JsonSerializer(message);
                        externalApiHandler[queueMessage.Menssge].SendMessageFromRemoteResource(queueMessage);
                    }
                }
            }
        }        
    }
}