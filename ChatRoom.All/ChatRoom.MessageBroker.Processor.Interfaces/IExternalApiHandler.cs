using ChatRoom.Common.QueueChatRoom;

namespace ChatRoom.MessageBroker.Processor.Interfaces
{
    public interface IExternalApiHandler
    {
        void SendMessageFromRemoteResource(QueueMessage queueMessage);
    }
}