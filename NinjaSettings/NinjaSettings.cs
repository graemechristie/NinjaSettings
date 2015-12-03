using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NinjaSettings.ValueConverters;
using ImpromptuInterface;
using NinjaSettings.Repositories;

namespace NinjaSettings
{

    public abstract class NinjaSettings : DynamicObject
    {
        private readonly IEnumerable<ISettingValueConverter> _defaultValueConverters = new ISettingValueConverter[]
        {
            new DateTimeValueConverter(), 
            new EnumValueConverter(),
            new ScalarValueConverter(),
            new CommaDelimitedScalarValueConverter(),
            new CommaDelimitedConstructorArgumentValueConverter() 
        };

        public IEnumerable<ISettingValueConverter> DefaultValueConverters
        {
            get { return _defaultValueConverters; }
        }
    }

    public class NinjaSettings<TSettings> : NinjaSettings
        where TSettings : class
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
        private readonly ISettingsRepository _settingsRepository;
        private readonly List<ISettingValueConverter> _settingValueConverters;

        public NinjaSettings()
            : this(new WebConfigAppSettingsRepository())
        {
        }

        public NinjaSettings(string[] args)
            : this(new CommandLineSettingsRepository(args), null)
        {
        }

        public NinjaSettings(ISettingsRepository settingsRepository)
            : this(settingsRepository, null)
        {
        }

        public NinjaSettings(IEnumerable<ISettingValueConverter> settingValueConverters)
            : this(new WebConfigAppSettingsRepository(), settingValueConverters)
        {
        }

        public NinjaSettings(ISettingsRepository settingsRepository,
            IEnumerable<ISettingValueConverter> settingValueConverters)
        {
            _settingsRepository = settingsRepository;
            _settingValueConverters = new List<ISettingValueConverter>(settingValueConverters ?? Enumerable.Empty<ISettingValueConverter>());

            _settingValueConverters.AddRange(DefaultValueConverters);
        }

        public TSettings Settings => this.ActLike<TSettings>();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            var propInfo = typeof (TSettings).GetProperty(name);

            if (propInfo == null)
                return base.TryGetMember(binder, out result);

            if (_cache.ContainsKey(name))
            {
                result = _cache[name];
                return true;
            }

            var fromValue = _settingsRepository.Get(name);
            if (fromValue == null)
            {
                result = GetDefault(propInfo.PropertyType);
                return true;
            }

            var converter = _settingValueConverters.FirstOrDefault(a => a.CanConvert(propInfo.PropertyType));
            if (converter == null)
                throw new InvalidOperationException($"No Value Converter found for setting type {propInfo.PropertyType.Name}");

            result = converter.Convert(fromValue, propInfo.PropertyType);
            _cache.Add(name, result);

            return true;
        }

        protected static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}

