using System;

namespace DynamicAppSettings
{
    public interface IAppSettingValueConverter
    {
        bool CanConvert(Type type);

        object Convert(string fromValue, Type convertToType);
    }
}