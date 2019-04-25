using ChatRoom.DataAccess.Dtos;
using ChatRoom.DataAccess.Interfaces;
using System;
using System.Collections.Generic;

namespace ChatRoom.DataAccess.Repositories
{
    public class ProfileRepository : BaseRepository<ProfileDto>, IProfileRepository
    {
        public ProfileRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public int DeleteByCode(string code)
        {
            var profile = GetByCode(code);
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            var db = this.GetConnection();
            var deletedRecord = db.Delete(profile);
            this.CloseConnection(db);
            return deletedRecord;
        }

        public IEnumerable<ProfileDto> GetAll()
        {
            try
            {
                var query = @"SELECT * FROM Profile";

                using (var db = dbFactory.GetConnection())
                {
                    return db.Fetch<ProfileDto>(query);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ProfileDto GetByCode(string code)
        {
            try
            {
                var query = @"SELECT * FROM Profile WHERE Code = @0";

                using (var db = dbFactory.GetConnection())
                {
                    return db.SingleOrDefault<ProfileDto>(query, code);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ProfileDto GetById(int id)
        {
            try
            {
                var query = @"SELECT * FROM Profile WHERE ProfileId = @0";

                using (var db = dbFactory.GetConnection())
                {
                    return db.SingleOrDefault<ProfileDto>(query, id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}