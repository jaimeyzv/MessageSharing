using ChatRoom.DataAccess.Interfaces;
using ChatRoom.Insfrastucture.Interfaces.Services;
using NPoco;
using Unity.Attributes;

namespace ChatRoom.DataAccess
{
    public class DbFactory : IDbFactory
    {
        private readonly string connectionString;
        private readonly IConnectionStringProvider connectionStringProvider;

        public DbFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        [InjectionConstructor]
        public DbFactory(IConnectionStringProvider connectionStringProvider)
        {
            this.connectionStringProvider = connectionStringProvider;
        }

        public IDatabase GetConnection()
        {
            var connString = connectionStringProvider != null && string.IsNullOrEmpty(connectionString) ? connectionStringProvider.GetConnectionString() : connectionString;
            return new Database(connString, "System.Data.SqlClient");
        }
    }
}