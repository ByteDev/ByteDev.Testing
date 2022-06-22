using System;
using System.Linq;
using System.Reflection;
using ByteDev.Collections;
using NUnit.Framework;

namespace ByteDev.Testing.UnitTests
{
    [TestFixture]
    public class TestSettingsTests
    {
        [TestFixture]
        public class Constructor : TestSettingsTests
        {
            [Test]
            public void WhenNoParam_ThenSetDefaults()
            {
                var sut = new TestSettings();

                Assert.That(sut.FilePaths, Is.Empty);
            }

            [Test]
            public void WhenAssemblyIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _ = new TestSettings(null));
            }

            [Test]
            public void WhenInit_ThenSetsFilePaths()
            {
                string userName = Environment.UserName;

                var sut = new TestSettings(Assembly.GetAssembly(typeof(TestSettingsTests)));

                var result = sut.FilePaths.ToList();

                Assert.That(result.Count, Is.EqualTo(5));
                Assert.That(result.First(), Is.EqualTo(@"C:\Temp\ByteDev.Testing.UnitTests.settings.json"));
                Assert.That(result.Second(), Is.EqualTo(@"C:\Dev\ByteDev.Testing.UnitTests.settings.json"));
                Assert.That(result.Third(), Is.EqualTo(@"Z:\Dev\ByteDev.Testing.UnitTests.settings.json"));
                Assert.That(result.Fourth(), Is.EqualTo(@"C:\Users\" + userName + @"\ByteDev.Testing.UnitTests.settings.json"));
                Assert.That(result.Fifth(), Is.EqualTo(@"C:\Users\" + userName + @"\Documents\ByteDev.Testing.UnitTests.settings.json"));
            }
        }

        [TestFixture]
        public class GetSettings : TestSettingsTests
        {
            [Test]
            public void WhenNoKvConfig_AndFilePathsIsEmpty_ThenThrowException()
            {
                var sut = new TestSettings(Assembly.GetAssembly(typeof(TestSettingsTests)));
                
                sut.FilePaths.Clear();
                sut.KeyVaultConfig.KeyVaultUri = null;

                var ex = Assert.Throws<TestingException>(() => _ = sut.GetSettings<object>());
                Assert.That(ex.Message, Is.EqualTo("Could not create new settings instance."));
            }
        }
    }
}