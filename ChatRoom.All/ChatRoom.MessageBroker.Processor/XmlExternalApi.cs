using ChatRoom.Common.QueueChatRoom;
using ChatRoom.MessageBroker.Processor.Interfaces;
using System;

namespace ChatRoom.MessageBroker.Processor
{
    public class XmlExternalApi : IExternalApiHandler
    {
        public void SendMessageFromRemoteResource(QueueMessage queueMessage)
        {
            throw new NotImplementedException();
        }
    }
}