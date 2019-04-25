using ChatRoom.Insfrastucture.Interfaces.Wrappers;
using System.Configuration;

namespace ChatRoom.Insfrastucture.Wrappers
{
    public class ConfigurationManagerWrapper : IConfigurationManagerWrapper
    {
        public string GetAppSettings(string path)
        {
            return ConfigurationManager.AppSettings[path];
        }

        public string GetAppSettings(string path, string defaultVal)
        {
            var val = GetAppSettings(path);
            return string.IsNullOrEmpty(val) ? defaultVal : val;
        }

        public string GetConnectionString(string name)
        {
            if (ConfigurationManager.ConnectionStrings[name] == null)
                throw new System.Exception($"Configuration error: connection string {name} not found");

            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}