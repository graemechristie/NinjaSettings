using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace NinjaSettings.ValueConverters
{
    public class CommaDelimitedConstructorArgumentValueConverter : ISettingValueConverter
    {
        public bool CanConvert(Type type)
        {
            if (type == typeof (string))
                return false;

            return type.GetInterfaces()
                //GHas Generic IEnumerable Interface
                .Where(i => i.IsGenericType
                            && typeof (IEnumerable<>).IsAssignableFrom(i.GetGenericTypeDefinition()))
                // and the type of that interface
                .Select(i => i.GetGenericArguments().FirstOrDefault())
                // has a constructor
                .Any(t => t != null &&
                          t.GetConstructors().Any(c =>
                              // with one argument
                              c.GetParameters().Length == 1
                              // That implements IConvertible, so it can be set by calling changetype on a string value
                              && c.GetParameters()[0].ParameterType.GetInterfaces().Any(ti => ti == typeof (IConvertible))));
        }

        public object Convert(string fromValue, Type convertToType)
        {
            var valueType = convertToType.GetInterfaces()
                .Where(i => i.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(i.GetGenericTypeDefinition()))
                .Select(i => i.GetGenericArguments().FirstOrDefault())
                .FirstOrDefault(t => t != null &&
                          t.GetConstructors().Any(c =>
                              c.GetParameters().Count() == 1
                              && c.GetParameters()[0].ParameterType.GetInterfaces().Any(ti => ti == typeof(IConvertible))));

            if (valueType == null)
                throw new ArgumentException("convertToType");

            var constructorArgumentType = valueType.GetConstructors()
                .Where(c =>
                    c.GetParameters().Count() == 1
                    && c.GetParameters()[0].ParameterType.GetInterfaces().Any(ti => ti == typeof(IConvertible)))
                .Select(c => c.GetParameters()[0].ParameterType)
                .FirstOrDefault();

            if (constructorArgumentType == null)
                throw new ArgumentException("convertToType");

            var values = fromValue.Split(',');

            if (convertToType.IsArray)
            {
                var arrayInstance = Activator.CreateInstance(convertToType, values.Count());
                var setValueMethod = convertToType.GetMethod("SetValue", new [] { typeof(object), typeof(Int32) });
                var i = 0;

                foreach (var v in values.Select(va=>va.Trim()))
                {
                    var constructorArgumentValue = System.Convert.ChangeType(v, constructorArgumentType);
                    var valueInstance = Activator.CreateInstance(valueType, constructorArgumentValue);
                    setValueMethod.Invoke(arrayInstance, new [] {valueInstance, i++ });
                }

                return arrayInstance;
            }

            var instance = Activator.CreateInstance(convertToType);

            var addMethod = convertToType.GetMethod("Add", new[] { valueType });

            foreach (var v in values.Select(va=>va.Trim()))
            {
                var constructorArgumentValue = System.Convert.ChangeType(v, constructorArgumentType);
                var valueInstance = Activator.CreateInstance(valueType, constructorArgumentValue);
                var parms = new [] { valueInstance };
                addMethod.Invoke(instance, parms);
            }

            return instance;
        }
    }
}