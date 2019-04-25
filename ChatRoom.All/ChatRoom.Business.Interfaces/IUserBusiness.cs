using ChatRoom.Business.Entities;
using System.Collections.Generic;

namespace ChatRoom.Business.Interfaces
{
    public interface IUserBusiness
    {
        int Create(UserEntity user);

        void Update(UserEntity user);

        void Delete(int id);

        UserEntity GetById(int id);

        UserEntity GetByNickName(string nickName);

        List<UserEntity> GetAll();
    }
}