using System.Threading.Tasks;
using ByteDev.Azure.KeyVault.Secrets;
using ByteDev.Testing.IntTests.TestFiles;
using ByteDev.Testing.Settings;
using ByteDev.Testing.Settings.Entities;
using ByteDev.Testing.Settings.Providers;
using ByteDev.Testing.Settings.Serialization;
using NUnit.Framework;

namespace ByteDev.Testing.IntTests.Settings
{
    [TestFixture]
    public class TestSettingsTests : TestBase
    {
        private const string SettingsFile = @"Z:\Dev\ByteDev.Testing.IntTests.json";

        [TestFixture]
        public class GetSettings_JsonFile : TestSettingsTests
        {
            private TestSettings _sut;

            [SetUp]
            public new void SetUp()
            {
                _sut = new TestSettings();
            }

            [Test]
            public void WhenSettingsFileDoesNotExist_ThenThrowException()
            {
                _sut.AddProvider(new JsonFileSettingsProvider(new[]
                {
                    GetFilePath(),
                    GetFilePath()
                }));

                var ex = Assert.Throws<TestingException>(() => _sut.GetSettings<DummyJsonFileSettings>());
                Assert.That(ex.Message, Is.EqualTo("Could not create new test settings instance."));
            }

            [Test]
            public void WhenSettingsIsNotValidJson_ThenThrowException()
            {
                _sut.AddProvider(new JsonFileSettingsProvider(TestFilePaths.InvalidJson));
                    
                var ex = Assert.Throws<TestingException>(() => _sut.GetSettings<DummyJsonFileSettings>());
                Assert.That(ex.Message, Is.EqualTo("Could not create new test settings instance."));
                Assert.That(ex.InnerException.Message, Is.EqualTo("Error while deserializing JSON settings in file: 'TestFiles\\InvalidJson.json'. Check JSON is valid."));
            }

            [Test]
            public void WhenPascalCaseSettingsFileExists_ThenReturnSettings()
            {
                _sut.AddProvider(new JsonFileSettingsProvider(new[]
                {
                    GetFilePath(), 
                    TestFilePaths.DummySettingsPascal
                }));

                var result = _sut.GetSettings<DummyJsonFileSettings>();

                Assert.That(result.KeyVaultName, Is.EqualTo("my-keyvault-pascal"));
                Assert.That(result.ClientId, Is.EqualTo("98a0d492-c6c6-4f1f-9d19-a98d94242ce6"));
            }

            [Test]
            public void WhenCamelCaseSettingsFileExists_ThenReturnSettings()
            {
                _sut.AddProvider(new JsonFileSettingsProvider(new[]
                {
                    GetFilePath(), 
                    TestFilePaths.DummySettingsCamel
                }));

                var result = _sut.GetSettings<DummyJsonFileSettings>();

                Assert.That(result.KeyVaultName, Is.EqualTo("my-keyvault-camel"));
                Assert.That(result.ClientId, Is.EqualTo("79714b64-d9b6-4d12-b107-adb6fa381bf3"));
            }
        }

        [TestFixture]
        public class GetSettings_KeyVault : TestSettingsTests
        {
            private TestSettings _sut;
            private TestAzureKeyVaultSettings _testAzureKeyVaultSettings;
            private IKeyVaultSecretClient _kvClient;

            [SetUp]
            public new void SetUp()
            {
                _testAzureKeyVaultSettings = JsonFileSettingsSerializer.Deserialize<TestAzureKeyVaultSettings>(SettingsFile);

                _kvClient = new KeyVaultSecretClient(_testAzureKeyVaultSettings.KeyVaultUri, _testAzureKeyVaultSettings.ToClientSecretCredential());

                _sut = new TestSettings();
            }

            [Test]
            public async Task WhenKvSettingMatchesPropertyName_ThenSetProperty()
            {
                await _kvClient.SafeSetValueAsync("ByteDevTesting--TestName", "John");

                _sut.AddProvider(new KeyVaultSettingsProvider(_kvClient, "ByteDevTesting--"));

                var result = _sut.GetSettings<DummyKeyVaultSettings>();

                Assert.That(result.TestName, Is.EqualTo("John"));
            }

            [Test]
            public void WhenKvSettingNotMatchesPropertyName_ThenDoNotSetProperty()
            {
                _sut.AddProvider(new KeyVaultSettingsProvider(_kvClient));

                var result = _sut.GetSettings<DummyKeyVaultSettings>();

                Assert.That(result.TestName, Is.Null);
            }

            [Test]
            public void WhenKvUriIsInvalid_ThenThrowException()
            {
                var kvClient = new KeyVaultSecretClient("https://blahblahsomethingkv345.vault.azure.net/", _testAzureKeyVaultSettings.ToClientSecretCredential());

                _sut.AddProvider(new KeyVaultSettingsProvider(kvClient));

                var ex = Assert.Throws<TestingException>(() => _sut.GetSettings<DummyKeyVaultSettings>());
                Assert.That(ex.Message, Is.EqualTo("Could not create new test settings instance."));
                Assert.That(ex.InnerException.Message, Is.EqualTo("Error while trying to deserialize settings object from Azure Key Vault settings."));
            }
        }

        [TestFixture]
        public class GetAzureSettings_JsonFile : TestSettingsTests
        {
            private TestSettings _sut;

            [SetUp]
            public new void SetUp()
            {
                _sut = new TestSettings();
            }

            [Test]
            public void WhenAllSettings_ThenSetAllSettings()
            {
                _sut.AddProvider(new JsonFileSettingsProvider(new[]
                {
                    TestFilePaths.AzureSettings
                }));

                var result = _sut.GetAzureSettings();

                Assert.That(result.ClientId, Is.EqualTo("someClientId"));
                Assert.That(result.ClientSecret, Is.EqualTo("someClientSecret"));
                Assert.That(result.SubscriptionId, Is.EqualTo("someSubscriptionId"));
                Assert.That(result.TenantId, Is.EqualTo("someTenantId"));
            }

            [Test]
            public void WhenSomeSettingsMissing_ThenSetMissingToNull()
            {
                _sut.AddProvider(new JsonFileSettingsProvider(new[]
                {
                    TestFilePaths.AzureSettingsPart
                }));

                var result = _sut.GetAzureSettings();

                Assert.That(result.ClientId, Is.Null);
                Assert.That(result.ClientSecret, Is.Null);
                Assert.That(result.SubscriptionId, Is.EqualTo("someSubscriptionId"));
                Assert.That(result.TenantId, Is.EqualTo("someTenantId"));
            }
        }
    }
}