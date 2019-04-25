using ChatRoom.Business.Entities;
using System.Collections.Generic;

namespace ChatRoom.Business.Interfaces
{
    public interface IMessageBusiness
    {
        int Create(MessageEntity message);
        void CreateMessageFromWorker(MessageEntity message);
        MessageEntity GetById(int id);
        List<MessageEntity> GetLatest50MessagesByChatroomId(int chatroomId);
        List<MessageEntity> GetAllMessagesByChatroomId(int chatroomId);
    }
}