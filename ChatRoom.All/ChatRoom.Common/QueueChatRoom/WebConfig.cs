using System.Configuration;

namespace ChatRoom.Common.QueueChatRoom
{
    public static class WebConfig
    {
        public static class RabbitMQ
        {
            public static string HostName { get { return ConfigurationManager.AppSettings["HostName"] ?? string.Empty; } }
            public static string QueueName { get { return ConfigurationManager.AppSettings["QueueName"] ?? string.Empty; } }
        }
    }
}