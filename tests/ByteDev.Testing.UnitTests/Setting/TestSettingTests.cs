using ByteDev.Testing.Setting;
using ByteDev.Testing.Setting.Providers;
using ByteDev.Testing.UnitTests.Settings;
using NSubstitute;
using NUnit.Framework;

namespace ByteDev.Testing.UnitTests.Setting
{
    [TestFixture]
    public class TestSettingTests
    {
        [TestFixture]
        public class Constructor : TestSettingsTests
        {
            private TestSetting _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new TestSetting();
            }

            [Test]
            public void WhenNoProvidersAdded_ThenThrowException()
            {
                var ex = Assert.Throws<TestingException>(() => _ = _sut.GetSetting());
                Assert.That(ex.Message, Is.EqualTo("No settings providers added. Add at least one."));
            }

            [Test]
            public void WhenOneProviderAdded_AndReturnsNull_ThenThrowException()
            {
                var provider = Substitute.For<ISettingProvider>();

                provider.GetSetting().Returns(null as string);

                _sut.AddProvider(provider);

                var ex = Assert.Throws<TestingException>(() => _ = _sut.GetSetting());
                Assert.That(ex.Message, Is.EqualTo("Could not create new test setting string."));
            }

            [Test]
            public void WhenTwoProvidersAdded_AndSecondReturnsInstance_ThenReturnInstance()
            {
                const string setting = "Test";
                
                var provider1 = Substitute.For<ISettingProvider>();
                var provider2 = Substitute.For<ISettingProvider>();

                provider1.GetSetting().Returns(null as string);
                provider2.GetSetting().Returns(setting);

                _sut.AddProvider(provider1);
                _sut.AddProvider(provider2);

                var result = _sut.GetSetting();

                Assert.That(result, Is.EqualTo(setting));
            }
        }
    }
}