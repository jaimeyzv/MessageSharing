namespace ChatRoom.Insfrastucture.Interfaces.Services
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();
        void SetConnectionString(string connectionString);
    }
}