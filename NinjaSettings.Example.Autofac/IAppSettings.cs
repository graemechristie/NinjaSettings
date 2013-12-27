using System;
using System.Collections.Generic;

namespace NinjaSettings.Example.Autofac
{
    public interface IAppSettings
    {
        List<int> SomeIntegerList { get; }
        TestEnum SomeTestEnum { get; }
        DateTime SomeDateValue { get; } 
        string[] SomeStringArray { get;  }
    }

}
