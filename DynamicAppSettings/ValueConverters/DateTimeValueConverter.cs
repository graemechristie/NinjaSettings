using System;
using System.Globalization;

namespace DynamicAppSettings.ValueConverters
{
    public class DateTimeValueConverter
        : BaseAppSettingsValueConverter<DateTime>
    {
        private readonly DateTimeFormatInfo _dateTimeFormatInfo;

        public DateTimeValueConverter()
            :this(CultureInfo.CurrentCulture)
        { 
        }

        public DateTimeValueConverter(CultureInfo cultureInfo)
        {
            _dateTimeFormatInfo = cultureInfo.DateTimeFormat;
        }

        public override DateTime Convert(string fromValue)
        {
            return DateTime.Parse(fromValue, _dateTimeFormatInfo);
        }
    }
}