using System;

namespace DynamicAppSettings.ValueConverters
{
    public abstract class BaseAppSettingsValueConverter<TConvertTo> : IAppSettingValueConverter
    {
        public bool CanConvert(Type type)
        {
            return typeof (TConvertTo).IsAssignableFrom(type);
        }

        public object Convert(string fromValue, Type convertToType)
        {
            return Convert(fromValue);
        }

        public abstract TConvertTo Convert(string fromValue);
    }
}