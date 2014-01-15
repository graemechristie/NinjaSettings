using System;
using System.Linq;

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
            if (String.IsNullOrEmpty(fromValue))
                return null;

            // If the user has supplied multiple values (and the type is a single enum)
            // we assume that this is a Flags enum and or the values together
            if (fromValue.Contains(","))
            {
                int e = 0;

                e = fromValue.Split(',')
                    .Select(s => (int)Enum.Parse(convertToType, s.Trim()))
                    .Aggregate(e, (current, next) => current | next);

                return Enum.ToObject(convertToType, e);
            }

            return Enum.Parse(convertToType, fromValue);
        }
    }
}