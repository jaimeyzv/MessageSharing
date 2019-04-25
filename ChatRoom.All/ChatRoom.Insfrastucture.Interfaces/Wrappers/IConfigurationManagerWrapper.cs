namespace ChatRoom.Insfrastucture.Interfaces.Wrappers
{
    public interface IConfigurationManagerWrapper
    {
        string GetAppSettings(string path);
        string GetAppSettings(string path, string defaultVal);
        string GetConnectionString(string name);
    }
}