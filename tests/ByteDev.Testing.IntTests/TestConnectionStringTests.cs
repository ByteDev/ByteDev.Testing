using System.IO;
using System.Reflection;
using ByteDev.Configuration.Environment;
using NUnit.Framework;

namespace ByteDev.Testing.IntTests
{
    [TestFixture]
    public class TestConnectionStringTests : TestBase
    {
        [TestFixture]
        public class GetValue : TestConnectionStringTests
        {
            private const string EnvVarName = "ByteDev-Testing-IntTests-ConnString";
            private const string ConnString = "DefaultEndpointsProtocol=https;AccountName=myaccount;AccountKey=someAccountKey;EndpointSuffix=core.windows.net";

            private TestConnectionString _sut;
            private EnvironmentVariableProvider _envProvider;

            [SetUp]
            public new void SetUp()
            {
                _envProvider = new EnvironmentVariableProvider();

                _sut = new TestConnectionString(Assembly.GetAssembly(typeof(TestConnectionStringTests)));
            }

            [TearDown]
            public new void TearDown()
            {
                _envProvider.Delete(EnvVarName);
            }

            [Test]
            public void WhenEnvVarSet_ThenReturnString()
            {
                _envProvider.Set(EnvVarName, ConnString);

                _sut.EnvironmentVarName = EnvVarName;

                var result = _sut.GetValue();

                Assert.That(result, Is.EqualTo(ConnString));
            }

            [Test]
            public void WhenPathSet_AndFileExist_ThenReturnString()
            {
                var fileInfo = CreateTextFile(ConnString);

                _sut.FilePaths = new []
                {
                    Path.Combine(ExistingDirectoryPath, Path.GetRandomFileName()),
                    fileInfo.FullName
                };

                var result = _sut.GetValue();

                Assert.That(result, Is.EqualTo(ConnString));
            }

            [Test]
            public void WhenPathsSet_AndNoFileExists_ThenThrowException()
            {
                _sut.FilePaths = new []
                {
                    Path.Combine(ExistingDirectoryPath, Path.GetRandomFileName()),
                    Path.Combine(ExistingDirectoryPath, Path.GetRandomFileName())
                };

                Assert.Throws<TestingException>(() => _sut.GetValue());
            }

            [Test]
            public void WhenNoFilePaths_AndNoEnvVar_ThenThrowException()
            {
                _sut.FilePaths.Clear();
                _sut.EnvironmentVarName = null;

                var ex = Assert.Throws<TestingException>(() => _sut.GetValue());
                Assert.That(ex.Message, Is.EqualTo("Could not find test connection string."));
            }
        }
    }
}