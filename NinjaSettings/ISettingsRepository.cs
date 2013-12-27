namespace NinjaSettings
{
    public interface ISettingsRepository
    {
        string Get(string settingName);
    }
}