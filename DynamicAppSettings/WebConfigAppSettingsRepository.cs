using System.Configuration;

namespace DynamicAppSettings
{
    public class WebConfigAppSettingsRepository : IAppSettingsRepository
    {
        public string Get(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }
    }
}