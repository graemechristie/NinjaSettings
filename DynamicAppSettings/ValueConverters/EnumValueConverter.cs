using System;

namespace DynamicAppSettings.ValueConverters
{
    public class EnumValueConverter
        : IAppSettingValueConverter

    {
        public bool CanConvert(Type type)
        {
            return type.IsEnum;
        }

        public object Convert(string fromValue, Type convertToType)
        {
            return Enum.Parse(convertToType, fromValue);
        }
    }
}