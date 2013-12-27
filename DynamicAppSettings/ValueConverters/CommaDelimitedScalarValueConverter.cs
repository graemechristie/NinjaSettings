using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicAppSettings.ValueConverters
{
    public class CommaDelimitedScalarValueConverter : IAppSettingValueConverter
    {
        public bool CanConvert(Type type)
        {
            if (type == typeof (string))
                return false;

            return type.GetInterfaces()
                .Where(i=>i.IsGenericType 
                    && typeof (IEnumerable<>).IsAssignableFrom(i.GetGenericTypeDefinition()))
                .Select(i => i.GetGenericArguments().FirstOrDefault())
                .Any(t => t != null && t.GetInterfaces().Any(ti=>ti == typeof(IConvertible)));
        }

        public object Convert(string fromValue, Type convertToType)
        {
            var typeParam = convertToType.GetInterfaces()
                .Where(i => i.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(i.GetGenericTypeDefinition()))
                .Select(i => i.GetGenericArguments().FirstOrDefault())
                .FirstOrDefault(t => t != null && t.GetInterfaces().Any(ti => ti == typeof(IConvertible)));

            if (typeParam == null)
                throw new ArgumentException("convertToType");

            var values = fromValue.Split(',');

            if (convertToType.IsArray)
            {
                var arrayInstance = Activator.CreateInstance(convertToType, values.Count());
                var setValueMethod = convertToType.GetMethod("SetValue", new [] { typeof(object), typeParam });
                var i = 0;

                foreach (var v in values.Select(va=>va.Trim()))
                {
                    setValueMethod.Invoke(arrayInstance, new object[] { System.Convert.ChangeType(v, typeParam), i++ });
                }

                return arrayInstance;
            }

            var instance = Activator.CreateInstance(convertToType);

            var addMethod = convertToType.GetMethod("Add", new[] { typeParam });

            foreach (var v in values.Select(va=>va.Trim()))
            {
                var parms = new [] { System.Convert.ChangeType(v, typeParam) };
                addMethod.Invoke(instance, parms);
            }

            return instance;
        }
    }
}