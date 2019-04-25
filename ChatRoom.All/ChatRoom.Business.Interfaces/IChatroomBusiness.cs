using ChatRoom.Business.Entities;

namespace ChatRoom.Business.Interfaces
{
    public interface IChatroomBusiness
    {
        int Create(ChatroomEntity chatroom);

        void Update(ChatroomEntity chatroom);

        void Delete(int id);

        ChatroomEntity GetById(int id);
    }
}