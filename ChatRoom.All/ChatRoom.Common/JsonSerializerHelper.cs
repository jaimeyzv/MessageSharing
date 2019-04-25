using ChatRoom.Common.QueueChatRoom;
using Newtonsoft.Json;

namespace ChatRoom.Common
{
    public static class JsonSerializerHelper
    {
        public static string JsonConvert(QueueMessage queueMessage)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(queueMessage);
        }

        public static QueueMessage JsonSerializer(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<QueueMessage>(json);
        }
    }
}