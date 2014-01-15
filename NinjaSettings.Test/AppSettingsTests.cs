using System;
using System.Collections.Generic;
using System.Globalization;
using NinjaSettings.ValueConverters;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace NinjaSettings.Test
{
    [TestFixture]
    public class AppSettingsTests
    {
        public enum TestEnum
        {
            Foo = 1,
            Bar = 2,
            Baz = 3
        }

        [Flags]
        public enum TestFlagsEnum
        {
            None = 0,
            One = 1,
            Two = 2,
            Four = 4,
            Eight = 8
        }

        public interface ITestAppSettings
        {
            string SomeString { get; }

            int SomeInteger { get; }

            Decimal SomeDecimal { get; }

            DateTime SomeDate { get;  }

            List<int> SomeIntegerList { get; }

            int[] SomeIntegerArray { get; }

            List<String> SomeStringList { get; }

            TestEnum SomeTestEnum { get; }            
            
            TestFlagsEnum SomeTestFlagsEnum { get; }

        }

        private ISettingsRepository _fakeRepository;


        [SetUp]
        public void Setup()
        {
            _fakeRepository = Substitute.For<ISettingsRepository>();
        }

        [Test]
        public void GivenAStringValueReturnsThatValueAsString()
        {
            _fakeRepository.Get("SomeString").Returns("SomeStringValue");

            var settings = new NinjaSettings<ITestAppSettings>(_fakeRepository).Settings;

            settings.SomeString.ShouldBe("SomeStringValue");
        }

        [Test]
        public void GivenAnIntegerValueReturnsThatValueAsInteger()
        {
            _fakeRepository.Get("SomeInteger").Returns("1");

            var settings = new NinjaSettings<ITestAppSettings>(_fakeRepository).Settings;

            settings.SomeInteger.ShouldBe(1);
        }

        [Test]
        public void GivenADecimalValueReturnsThatValueAsDecimal()
        {
            _fakeRepository.Get("SomeDecimal").Returns("1.23");

            var settings = new NinjaSettings<ITestAppSettings>(_fakeRepository).Settings;

            settings.SomeDecimal.ShouldBe(1.23M);
        }

        [Test]
        public void GivenADateValueReturnsThatValueAsDate()
        {
            _fakeRepository.Get("SomeDate").Returns("12/11/2013");

            // The Default DateTimeValueConverter uses the current culture to parse the date
            // so we supply one with the culture explicity set to AU.
            // This will be prefered over the default one.
            var cultureInfo = new CultureInfo("en-au", false);
            var valueConverters = new ISettingValueConverter[] {new DateTimeValueConverter(cultureInfo)}; 
            var settings = new NinjaSettings<ITestAppSettings>(_fakeRepository, valueConverters).Settings;

            settings.SomeDate.ShouldBe(new DateTime(2013, 11, 12));
        }

        [Test]
        public void GivenAnEnumValueReturnsThatValueAsEnum()
        {
            _fakeRepository.Get("SomeTestEnum").Returns("Baz");

            var settings = new NinjaSettings<ITestAppSettings>(_fakeRepository).Settings;

            settings.SomeTestEnum.ShouldBe(TestEnum.Baz);
        }

        [Test]
        public void GivenMultipleEnumValueReturnsThatValueAsEnumFlags()
        {
            _fakeRepository.Get("SomeTestFlagsEnum").Returns("Two, Four");

            var settings = new NinjaSettings<ITestAppSettings>(_fakeRepository).Settings;

            settings.SomeTestFlagsEnum.ShouldBe(TestFlagsEnum.Two | TestFlagsEnum.Four );
        }

        [Test]
        public void GivenAnIntegerEnumerableValueReturnsThatValueAsAnIntegerList()
        {
            _fakeRepository.Get("SomeIntegerList").Returns("1,2,34,99,88");

            var settings = new NinjaSettings<ITestAppSettings>(_fakeRepository).Settings;

            settings.SomeIntegerList.ShouldBe(new List<int>  {1,2,34,99,88 } );
        }

        [Test]
        public void GivenAnIntegerEnumerableValueReturnsThatValueAsAnIntegerArray()
        {
            _fakeRepository.Get("SomeIntegerArray").Returns("1,2,34,99,88");

            var settings = new NinjaSettings<ITestAppSettings>(_fakeRepository).Settings;

            settings.SomeIntegerArray.ShouldBe(new[] { 1, 2, 34, 99, 88 });
        }

        [Test]
        public void GivenAStringEnumerableValueReturnsThatValueAsAStringList()
        {
            _fakeRepository.Get("SomeStringList").Returns("foo,bar,baz, beep, bop");

            var settings = new NinjaSettings<ITestAppSettings>(_fakeRepository).Settings;

            settings.SomeStringList.ShouldBe(new List<string>(new[] { "foo", "bar","baz", "beep", "bop" }));
        }
    }
}
