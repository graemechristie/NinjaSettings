using System;

namespace NinjaSettings
{
    public interface ISettingValueConverter
    {
        bool CanConvert(Type type);

        object Convert(string fromValue, Type convertToType);
    }
}