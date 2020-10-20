using System.Reflection;
using NUnit.Framework;

namespace ByteDev.Testing.IntTests
{
    [TestFixture]
    public class TestSettingsTests : TestBase
    {
        private const string TestFilePath = @"TestFiles\DummySettings.json";
        private const string AzureSettingsTestFilePath = @"TestFiles\AzureSettings.json";
        private const string AzureSettingsTestPartFilePath = @"TestFiles\AzureSettingsPart.json";

        private TestSettings _sut;

        [SetUp]
        public new void SetUp()
        {
            _sut = new TestSettings(Assembly.GetAssembly(typeof(TestSettingsTests)));
        }

        [Test]
        public void WhenSettingsFileExists_ThenReturnSettings()
        {
            _sut.FilePaths = new[]
            {
                GetFilePath(),
                TestFilePath
            };

            var result = _sut.GetSettings<DummySettings>();

            Assert.That(result.KeyVaultName, Is.EqualTo("my-keyvault"));
            Assert.That(result.ClientId, Is.EqualTo("98a0d492-c6c6-4f1f-9d19-a98d94242ce6"));
        }

        [Test]
        public void WhenSettingsFileDoesNotExist_ThenThrowException()
        {
            _sut.FilePaths = new[]
            {
                GetFilePath(),
                GetFilePath()
            };

            Assert.Throws<TestingException>(() => _sut.GetSettings<DummySettings>());
        }

        [Test]
        public void WhenUsingTestAzureSettings_ThenReturnSettings()
        {
            _sut.FilePaths = new[]
            {
                AzureSettingsTestFilePath
            };

            var result = _sut.GetSettings<TestAzureSettings>();

            Assert.That(result.ClientId, Is.EqualTo("someClientId"));
            Assert.That(result.ClientSecret, Is.EqualTo("someClientSecret"));
            Assert.That(result.SubscriptionId, Is.EqualTo("someSubscriptionId"));
            Assert.That(result.TenantId, Is.EqualTo("someTenantId"));
        }

        [Test]
        public void WhenUsingTestAzureSettings_AndSomeSettingsMissing_ThenReturnSettings()
        {
            _sut.FilePaths = new[]
            {
                AzureSettingsTestPartFilePath
            };

            var result = _sut.GetSettings<TestAzureSettings>();

            Assert.That(result.ClientId, Is.Null);
            Assert.That(result.ClientSecret, Is.Null);
            Assert.That(result.SubscriptionId, Is.EqualTo("someSubscriptionId"));
            Assert.That(result.TenantId, Is.EqualTo("someTenantId"));
        }
    }

    public class DummySettings
    {
        public string KeyVaultName { get; set; }

        public string ClientId { get; set; }
    }
}