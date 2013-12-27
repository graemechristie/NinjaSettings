
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

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
