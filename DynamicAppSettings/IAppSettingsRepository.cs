namespace DynamicAppSettings
{
    public interface IAppSettingsRepository
    {
        string Get(string settingName);
    }
}