NinjaSettings
================

Simple Dynamic Wrapper around the .NET web.config app settings that gives strongly typed access to app settings. Easily mockable and extensible. 


## Quick Example 

Add some ordinary old app settings to your web.config file.

```xml
  <appSettings>
    <add key="SomeIntegerList" value="50,20,10,100"/>
    <add key="SomeTestEnum" value="Bar"/>
    <add key="SomeDateValue" value ="11/10/2013"/>
    <add key="SomeStringArray" value ="bing,bam,  boom, buzz"/> 
  </appSettings>

```

Create an interface with property names matching the app settings keys, and the desired types you'd like the values converted to when you reference them in your application.

```C#
 public interface IAppSettings
    {
        List<int> SomeIntegerList { get; }
        TestEnum SomeTestEnum { get; }
        DateTime SomeDateValue { get; } 
        string[] SomeStringArray { get;  }
    }
```

Register your interface with your favourite IOC container, using the NinjaSettings wrapper.

```C#
        protected void Application_Start()
        {
            //Autofac Setup
            var builder = new ContainerBuilder();
            builder.Register(c => new NinjaSettings<IAppSettings>().Settings)
                .As<IAppSettings>().SingleInstance();
		}
```

Simply inject your app settings into your classes as a dependency on your interface. Access your settings as a strongly typed object with full intellisense.


```C#
    public class MyService
    {
        public readonly IAppSettings _appSettings;

        public MyService(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public void DoSomeStuff()
        {
            foreach (var s in _appSettings.SomeStringArray)
            {
                // Do Something
            }
        }
    } 
```

