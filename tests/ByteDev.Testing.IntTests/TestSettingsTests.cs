using System.Reflection;
using NUnit.Framework;

namespace ByteDev.Testing.IntTests
{
    [TestFixture]
    public class TestSettingsTests : TestBase
    {
        private const string TestFilePath = @"TestFiles\DummySettings.json";

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
        public void WhenSettingsFilleDoesNotExist_ThenThrowException()
        {
            _sut.FilePaths = new[]
            {
                GetFilePath(),
                GetFilePath()
            };

            Assert.Throws<TestingException>(() => _sut.GetSettings<DummySettings>());
        }
    }

    public class DummySettings
    {
        public string KeyVaultName { get; set; }

        public string ClientId { get; set; }
    }
}