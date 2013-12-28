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

Register your interface with your favourite IOC container, using the `NinjaSettings` wrapper.

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
    private readonly IAppSettings _appSettings;

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

No need for your web.config or the `NinjaSettings` wrapper for your tests. Just Mock up an instance of your interface and pass that in to your class under test.

```C#
[Test]
public void GivenMyServiceDoesSomethingThenItShouldGoBeep()
{
    var mockAppSettings = Substitute.For<IAppSettings>();
    mockAppSettings.SomeStringArray.Returns(new[] {"foo", "bar", "baz"});

    var classUnderTest = new MyService(mockAppSettings);

    classUnderTest.DoSomeStuff();

    Assert.IsTrue(classUnderTest.HasGoneBeep);
}
```

## Ok, so whats this about being extensible

The `NinjaSettings` wrapper can be extended to parse just about any type. Out of the box it handles converting string settings to scalar values, collections of scalar values, `Enum`s and `DateTime`s. Just about any conversion from the original string can be handled by creating an implementing a simple value converter. 

```
public interface ISettingValueConverter
{
    bool CanConvert(Type type);

    object Convert(string fromValue, Type convertToType);
}
```

By default, `NinjaSettings` wraps the `<AppSettings>` section of the ASP.NET web.config file, but can easily be used to wrap any key-value store (resource (resx) files, cache entries, string dictionaries, look-up tables in databases) by implementing the following super simple Interface.

```
public interface ISettingsRepository
{
    string Get(string settingName);
}
```

Just pass your `ISettingsRepository` and/or an `IEnumerable<ISettingValueConverter>` into the `NinjaSettings` constructor.


## How do I get it

Clone this repository and build it yourself, or get it from [Nuget](https://www.nuget.org/packages/NinjaSettings/ "Nuget")

```
Install-Package NinjaSettings
```


