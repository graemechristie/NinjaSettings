using System.Configuration;

namespace NinjaSettings
{
    public class WebConfigAppSettingsRepository : ISettingsRepository
    {
        public string Get(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }
    }
}