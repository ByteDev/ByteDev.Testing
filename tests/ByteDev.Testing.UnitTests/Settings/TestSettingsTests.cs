using ByteDev.Testing.Settings;
using ByteDev.Testing.Settings.Providers;
using NSubstitute;
using NUnit.Framework;

namespace ByteDev.Testing.UnitTests.Settings
{
    [TestFixture]
    public class TestSettingsTests
    {
        [TestFixture]
        public class GetSettings : TestSettingsTests
        {
            private TestSettings _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new TestSettings();
            }

            [Test]
            public void WhenNoProvidersAdded_ThenThrowException()
            {
                var ex = Assert.Throws<TestingException>(() => _ = _sut.GetSettings<object>());
                Assert.That(ex.Message, Is.EqualTo("No settings providers added. Add at least one."));
            }

            [Test]
            public void WhenOneProviderAdded_AndReturnsNull_ThenThrowException()
            {
                var provider = Substitute.For<ISettingsProvider>();

                provider.GetSettings<object>().Returns(null);

                _sut.AddProvider(provider);

                var ex = Assert.Throws<TestingException>(() => _ = _sut.GetSettings<object>());
                Assert.That(ex.Message, Is.EqualTo("Could not create new test settings instance."));
            }

            [Test]
            public void WhenTwoProvidersAdded_AndSecondReturnsInstance_ThenReturnInstance()
            {
                var settings = new object();
                
                var provider1 = Substitute.For<ISettingsProvider>();
                var provider2 = Substitute.For<ISettingsProvider>();

                provider1.GetSettings<object>().Returns(null);
                provider2.GetSettings<object>().Returns(settings);

                _sut.AddProvider(provider1);
                _sut.AddProvider(provider2);

                var result = _sut.GetSettings<object>();

                Assert.That(result, Is.SameAs(settings));
            }
        }
    }
}