using ChatRoom.Common.QueueChatRoom;

namespace ChatRoom.MessageBroker.Interfaces
{
    public interface IQueueHandler
    {
        void SendMessage(string queueName, QueueMessage queueMessage);
    }
}