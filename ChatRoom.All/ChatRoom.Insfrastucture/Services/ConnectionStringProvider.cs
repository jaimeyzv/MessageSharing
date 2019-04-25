using ChatRoom.Insfrastucture.Interfaces.Services;
using ChatRoom.Insfrastucture.Interfaces.Wrappers;

namespace ChatRoom.Insfrastucture.Services
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfigurationManagerWrapper _configurationManagerWrapper;

        public ConnectionStringProvider(IConfigurationManagerWrapper configurationManagerWrapper)
        {
            _configurationManagerWrapper = configurationManagerWrapper;
        }
        public string GetConnectionString()
        {
            return _configurationManagerWrapper.GetAppSettings("ChatRoomDB", string.Empty);
        }

        public void SetConnectionString(string connectionString)
        {
            throw new System.NotImplementedException();
        }
    }
}