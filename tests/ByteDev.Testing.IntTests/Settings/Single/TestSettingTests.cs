using ByteDev.Configuration.Environment;
using ByteDev.Testing.Settings.Single;
using ByteDev.Testing.Settings.Single.Providers;
using NUnit.Framework;

namespace ByteDev.Testing.IntTests.Settings.Single
{
    [TestFixture]
    public class TestSettingTests : TestBase
    {
        private TestSetting _sut;

        [SetUp]
        public new void SetUp()
        {
            _sut = new TestSetting();
        }

        [TestFixture]
        public class GetSetting_File : TestSettingTests
        {
            [Test]
            public void WhenProvidersFailToFindSetting_ThenThrowException()
            {
                _sut.AddProvider(new FileSettingProvider(new[]
                {
                    GetFilePath(),
                    GetFilePath()
                }));

                var ex = Assert.Throws<TestingException>(() => _sut.GetSetting());
                Assert.That(ex.Message, Is.EqualTo("Could not create new test setting string."));
            }

            [Test]
            public void WhenFileIsEmpty_ThenThrowException()
            {
                var file = CreateTextFile();

                _sut.AddProvider(new FileSettingProvider(file));

                var ex = Assert.Throws<TestingException>(() => _sut.GetSetting());
                Assert.That(ex.Message, Is.EqualTo("Could not create new test setting string."));
            }

            [Test]
            public void WhenFileExists_ThenReturnSetting()
            {
                const string content = "Test setting";

                var file = CreateTextFile(content);

                _sut.AddProvider(new FileSettingProvider(new[]
                {
                    GetFilePath(),
                    file.FullName
                }));

                var result = _sut.GetSetting();

                Assert.That(result, Is.EqualTo(content));
            }
        }

        [TestFixture]
        public class GetSetting_EnvVar : TestSettingTests
        {
            private const string VarName = "ByteDevTestingEnvVar";
            private const string VarValue = "John";

            [SetUp]
            public new void SetUp()
            {
                new EnvironmentVariableProvider().Delete(VarName);
            }

            [OneTimeTearDown]
            public void ClassTearDown()
            {
                new EnvironmentVariableProvider().Delete(VarName);
            }

            [Test]
            public void WhenEnvVarNotExist_ThenThrowException()
            {
                _sut.AddProvider(new EnvironmentSettingProvider("cdd30206a7ce49f5a6977"));

                var ex = Assert.Throws<TestingException>(() => _sut.GetSetting());
                Assert.That(ex.Message, Is.EqualTo("Could not create new test setting string."));
            }

            [Test]
            public void WhenEnvVarDoesExist_ThenReturnSetting()
            {
                var provider = new EnvironmentVariableProvider();
                provider.Set(VarName, VarValue);

                _sut.AddProvider(new EnvironmentSettingProvider(new[]
                {
                    "cdd30206a7ce49f5a6977", VarName
                }));

                var result = _sut.GetSetting();

                Assert.That(result, Is.EqualTo(VarValue));
            }
        }
    }
}