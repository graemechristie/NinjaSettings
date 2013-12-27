using System;
using System.Linq;

namespace DynamicAppSettings.ValueConverters
{
    public class ScalarValueConverter : IAppSettingValueConverter
    {
        public bool CanConvert(Type type)
        {
            return type.GetInterfaces().Any(i=>i == typeof(IConvertible));
        }

        public object Convert(string fromValue, Type convertToType)
        {
            return System.Convert.ChangeType(fromValue, convertToType);
        }
    }
}