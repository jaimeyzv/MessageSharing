using NPoco;

namespace ChatRoom.DataAccess.Interfaces
{
    public interface IDbFactory
    {
        IDatabase GetConnection();
    }
}