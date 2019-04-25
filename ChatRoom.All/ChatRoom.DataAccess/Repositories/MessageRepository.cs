using ChatRoom.DataAccess.Dtos;
using ChatRoom.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace ChatRoom.DataAccess.Repositories
{
    public class MessageRepository : BaseRepository<MessageDto>, IMessageRepository
    {
        public MessageRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<MessageDto> GetAll()
        {
            try
            {
                var query = @"SELECT * FROM [dbo].[Message]";

                using (var db = dbFactory.GetConnection())
                {
                    return db.Fetch<MessageDto>(query);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public MessageDto GetById(int id)
        {
            try
            {
                var query = @"SELECT * FROM [dbo].[Message] WHERE MessageId = @0";

                using (var db = dbFactory.GetConnection())
                {
                    return db.SingleOrDefault<MessageDto>(query, id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<MessageDto> GetAllMessagesByChatroomId(int chatroomId)
        {
            try
            {
                var query = @"SELECT * FROM [dbo].[Message] WHERE ChatroomId = @0 ORDER BY DATE ASC";

                using (var db = dbFactory.GetConnection())
                {
                    return db.Fetch<MessageDto>(query, chatroomId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public IEnumerable<MessageDto> GetLatest50MessagesByChatroomId(int chatroomId)
        {
            try
            {
                var query = @"SELECT TOP 50 * FROM [dbo].[Message] WHERE ChatroomId = @0 ORDER BY DATE ASC";

                using (var db = dbFactory.GetConnection())
                {
                    return db.Fetch<MessageDto>(query, chatroomId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}