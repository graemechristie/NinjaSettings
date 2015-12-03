using System.Configuration;

namespace NinjaSettings.Repositories
{
    public class WebConfigAppSettingsRepository : ISettingsRepository
    {
        public string Get(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }
    }
}