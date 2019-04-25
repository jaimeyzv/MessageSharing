using ChatRoom.DataAccess.Dtos;

namespace ChatRoom.DataAccess.Interfaces
{
    public interface IProfileRepository : IBaseRepository<ProfileDto>
    {
        ProfileDto GetByCode(string code);
        int DeleteByCode(string code);
    }
}