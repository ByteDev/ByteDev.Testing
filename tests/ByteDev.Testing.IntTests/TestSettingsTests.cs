using System.Reflection;
using ByteDev.Testing.IntTests.TestFiles;
using NUnit.Framework;

namespace ByteDev.Testing.IntTests
{
    [TestFixture]
    public class TestSettingsTests : TestBase
    {
        private TestSettings _sut;

        [SetUp]
        public new void SetUp()
        {
            _sut = new TestSettings(Assembly.GetAssembly(typeof(TestSettingsTests)));
        }

        [TestFixture]
        public class GetSettings : TestSettingsTests
        {
            [Test]
            public void WhenSettingsFileDoesNotExist_ThenThrowException()
            {
                _sut.FilePaths = new[]
                {
                    GetFilePath(),
                    GetFilePath()
                };

                var ex = Assert.Throws<TestingException>(() => _sut.GetSettings<DummySettings>());
                Assert.That(ex.Message, Is.EqualTo("Could not find test settings file."));
            }

            [Test]
            public void WhenSettingsIsNotValidJson_ThenThrowException()
            {
                _sut.FilePaths = new[]
                {
                    TestFilePaths.InvalidJson
                };

                var ex = Assert.Throws<TestingException>(() => _sut.GetSettings<DummySettings>());
                Assert.That(ex.Message, Is.EqualTo("Error while deserializing JSON settings in file: 'TestFiles\\InvalidJson.json'. Check JSON is valid."));
            }

            [Test]
            public void WhenPascalCaseSettingsFileExists_ThenReturnSettings()
            {
                _sut.FilePaths = new[]
                {
                    GetFilePath(), TestFilePaths.DummySettingsPascal
                };

                var result = _sut.GetSettings<DummySettings>();

                Assert.That(result.KeyVaultName, Is.EqualTo("my-keyvault-pascal"));
                Assert.That(result.ClientId, Is.EqualTo("98a0d492-c6c6-4f1f-9d19-a98d94242ce6"));
            }

            [Test]
            public void WhenCamelCaseSettingsFileExists_ThenReturnSettings()
            {
                _sut.FilePaths = new[]
                {
                    GetFilePath(), TestFilePaths.DummySettingsCamel
                };

                var result = _sut.GetSettings<DummySettings>();

                Assert.That(result.KeyVaultName, Is.EqualTo("my-keyvault-camel"));
                Assert.That(result.ClientId, Is.EqualTo("79714b64-d9b6-4d12-b107-adb6fa381bf3"));
            }
        }

        [TestFixture]
        public class GetAzureSettings : TestSettingsTests
        {
            [Test]
            public void WhenAllSettings_ThenSetAllSettings()
            {
                _sut.FilePaths = new[]
                {
                    TestFilePaths.AzureSettings
                };

                var result = _sut.GetAzureSettings();

                Assert.That(result.ClientId, Is.EqualTo("someClientId"));
                Assert.That(result.ClientSecret, Is.EqualTo("someClientSecret"));
                Assert.That(result.SubscriptionId, Is.EqualTo("someSubscriptionId"));
                Assert.That(result.TenantId, Is.EqualTo("someTenantId"));
            }

            [Test]
            public void WhenSomeSettingsMissing_ThenSetMissingToNull()
            {
                _sut.FilePaths = new[]
                {
                    TestFilePaths.AzureSettingsPart
                };

                var result = _sut.GetAzureSettings();

                Assert.That(result.ClientId, Is.Null);
                Assert.That(result.ClientSecret, Is.Null);
                Assert.That(result.SubscriptionId, Is.EqualTo("someSubscriptionId"));
                Assert.That(result.TenantId, Is.EqualTo("someTenantId"));
            }
        }
    }
}