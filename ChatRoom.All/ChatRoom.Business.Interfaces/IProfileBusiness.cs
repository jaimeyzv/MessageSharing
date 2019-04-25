using ChatRoom.Business.Entities;
using System.Collections.Generic;

namespace ChatRoom.Business.Interfaces
{
    public interface IProfileBusiness
    {
        int Create(ProfileEntity profile);

        void Update(ProfileEntity profile);

        void Delete(int id);

        void DeleteByCode(string code);

        ProfileEntity GetByCode(string code);

        List<ProfileEntity> GetAll();
    }
}