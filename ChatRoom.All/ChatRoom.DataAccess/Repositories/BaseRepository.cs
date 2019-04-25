using ChatRoom.DataAccess.Interfaces;
using NPoco;
using System;

namespace ChatRoom.DataAccess.Repositories
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly IDbFactory dbFactory;
        protected Snapshot<T> snapShot;
        protected string databaseName;

        public BaseRepository(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public virtual object Insert(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));
                var db = this.GetConnection();
                var insertedRecord = db.Insert(entity);
                this.CloseConnection(db);
                return insertedRecord;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual object Delete(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));
                var db = this.GetConnection();
                var deletedRecord = db.Delete(entity);
                this.CloseConnection(db);
                return deletedRecord;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual object Delete(object primaryKey)
        {
            try
            {
                if (primaryKey == null) throw new ArgumentNullException(nameof(primaryKey));
                var db = this.GetConnection();
                var deletedRecord = db.Delete<T>(primaryKey);
                this.CloseConnection(db);
                return deletedRecord;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual object Update(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));
                var db = this.GetConnection();
                if (snapShot == null)
                {
                    var updatedRecord = db.Update(entity);
                    this.CloseConnection(db);
                    return updatedRecord;
                }
                else
                {
                    var updatedRecord = db.Update(entity, snapShot.UpdatedColumns());
                    this.CloseConnection(db);
                    return updatedRecord;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual void StartSnapshot(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));
                var db = this.GetConnection();
                snapShot = db.StartSnapshot(entity);
                this.CloseConnection(db);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected IDatabase GetConnection()
        {
            var connection = dbFactory.GetConnection();
            connection.OpenSharedConnection();
            if (string.IsNullOrEmpty(databaseName) || databaseName.ToLower() == connection.Connection.Database.ToLower()) return connection;
            connection.Connection.ChangeDatabase(databaseName);
            return connection;
        }

        protected void CloseConnection(IDatabase connection)
        {
            connection.CloseSharedConnection();
            connection.Dispose();
        }
    }
}
