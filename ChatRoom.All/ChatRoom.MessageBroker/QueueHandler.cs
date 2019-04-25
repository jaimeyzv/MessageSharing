using ChatRoom.Common;
using ChatRoom.Common.QueueChatRoom;
using ChatRoom.MessageBroker.Interfaces;
using RabbitMQ.Client;
using System;
using System.Text;

namespace ChatRoom.MessageBroker
{
    public class QueueHandler : IQueueHandler
    {
        public void SendMessage(string queueName, QueueMessage queueMessage)
        {
            if (string.IsNullOrEmpty(queueName)) throw new Exception("Queue name is required.");
            if (queueMessage == null) throw new ArgumentNullException("Queue message is required.");

            var factory = new ConnectionFactory() { HostName = WebConfig.RabbitMQ.HostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queueName, false, false, false, null);
                    var message = JsonSerializerHelper.JsonConvert(queueMessage);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(string.Empty, queueName, null, body);
                }
            }
        }
    }
}