using ChatRoom.DataAccess.Dtos;

namespace ChatRoom.DataAccess.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserDto>
    {
        UserDto GetByNickName(string nickName);
    }
}