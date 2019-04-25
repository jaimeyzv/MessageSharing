using ChatRoom.DataAccess.Dtos;
using ChatRoom.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace ChatRoom.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<UserDto>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<UserDto> GetAll()
        {
            try
            {
                var query = @"SELECT * FROM [dbo].[User]";

                using (var db = dbFactory.GetConnection())
                {
                    return db.Fetch<UserDto>(query);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public UserDto GetById(int id)
        {
            try
            {
                var query = @"SELECT * FROM [dbo].[User] WHERE UserId = @0";

                using (var db = dbFactory.GetConnection())
                {
                    return db.SingleOrDefault<UserDto>(query, id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public UserDto GetByNickName(string nickName)
        {
            try
            {
                var query = @"SELECT * FROM [dbo].[User] WHERE NickName = @0";

                using (var db = dbFactory.GetConnection())
                {
                    return db.SingleOrDefault<UserDto>(query, nickName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}