using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management.Instrumentation;
using DynamicAppSettings.ValueConverters;
using ImpromptuInterface;

namespace DynamicAppSettings
{

    public abstract class DynamicAppSettings : DynamicObject
    {
        private readonly IEnumerable<IAppSettingValueConverter> _defaultValueConverters = new IAppSettingValueConverter[]
        {
            new DateTimeValueConverter(), 
            new EnumValueConverter(),
            new ScalarValueConverter(),
            new CommaDelimitedScalarValueConverter()
        };

        public IEnumerable<IAppSettingValueConverter> DefaultValueConverters
        {
            get { return _defaultValueConverters; }
        }
    }

    public class DynamicAppSettings<TAppSettings> : DynamicAppSettings
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
        private readonly IAppSettingsRepository _appSettingsRepository;
        private readonly List<IAppSettingValueConverter> _appSettingValueConverters;

        public DynamicAppSettings()
            : this(new WebConfigAppSettingsRepository())
        {
        }

        public DynamicAppSettings(IAppSettingsRepository appSettingsRepository)
            : this(appSettingsRepository, null)
        {
        }

        public DynamicAppSettings(IEnumerable<IAppSettingValueConverter> appSettingValueConverters)
            : this(new WebConfigAppSettingsRepository(), appSettingValueConverters)
        {
        }

        public DynamicAppSettings(IAppSettingsRepository appSettingsRepository,
            IEnumerable<IAppSettingValueConverter> appSettingValueConverters)
        {
            _appSettingsRepository = appSettingsRepository;

            _appSettingValueConverters = new List<IAppSettingValueConverter>(appSettingValueConverters ?? Enumerable.Empty<IAppSettingValueConverter>());

            _appSettingValueConverters.AddRange(DefaultValueConverters);
        }

        public TAppSettings AppSettings
        {
            get
            {
                return this.ActLike(new[] {typeof (TAppSettings)});
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;

            var propInfo = typeof (TAppSettings).GetProperty(name);

            if (propInfo == null)
                return base.TryGetMember(binder, out result);

            if (_cache.ContainsKey(name))
            {
                result = _cache[name];
                return true;
            }

            var fromValue = _appSettingsRepository.Get(name);

            var converter = _appSettingValueConverters.FirstOrDefault(a => a.CanConvert(propInfo.PropertyType));

            if (converter == null)
                throw new InvalidOperationException(String.Format("No Value Converte found for application setting type {0}", propInfo.PropertyType.Name));

            result = converter.Convert(fromValue, propInfo.PropertyType);

            _cache.Add(name, result);

            return true;
        }
    }
}

