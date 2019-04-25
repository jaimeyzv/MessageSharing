using ChatRoom.DataAccess.Dtos;
using ChatRoom.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace ChatRoom.DataAccess.Repositories
{
    public class ChatroomRepository : BaseRepository<ChatroomDto>, IChatroomRepository
    {
        public ChatroomRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<ChatroomDto> GetAll()
        {
            try
            {
                var query = @"SELECT * FROM [dbo].[Chatroom]";

                using (var db = dbFactory.GetConnection())
                {
                    return db.Fetch<ChatroomDto>(query);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ChatroomDto GetById(int id)
        {
            try
            {
                var query = @"SELECT * FROM [dbo].[Chatroom] WHERE ChatroomId = @0";

                using (var db = dbFactory.GetConnection())
                {
                    return db.SingleOrDefault<ChatroomDto>(query, id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}