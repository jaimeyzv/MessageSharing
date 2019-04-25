using ChatRoom.DataAccess.Dtos;
using System.Collections.Generic;

namespace ChatRoom.DataAccess.Interfaces
{
    public interface IMessageRepository : IBaseRepository<MessageDto>
    {
        IEnumerable<MessageDto> GetLatest50MessagesByChatroomId(int chatroomId);
        IEnumerable<MessageDto> GetAllMessagesByChatroomId(int chatroomId);
    }
}