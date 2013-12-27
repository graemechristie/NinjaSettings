using System;

namespace NinjaSettings.ValueConverters
{
    public class EnumValueConverter
        : ISettingValueConverter

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