using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace ByteDev.Testing.IntTests
{
    [TestFixture]
    public class TestConnectionStringTests : TestBase
    {
        private TestConnectionString _sut;

        [SetUp]
        public new void SetUp()
        {
            _sut = new TestConnectionString(Assembly.GetAssembly(typeof(TestConnectionStringTests)));
        }

        [TestFixture]
        public class GetConnectionString : TestConnectionStringTests
        {
            private const string EnvVarName = "ByteDev-Testing-IntTests-ConnString";
            private const string ConnString = "DefaultEndpointsProtocol=https;AccountName=myaccount;AccountKey=someAccountKey;EndpointSuffix=core.windows.net";

            [TearDown]
            public new void TearDown()
            {
                Environment.SetEnvironmentVariable(EnvVarName, null);
            }

            [Test]
            public void WhenEnvVarSet_ThenReturnString()
            {
                Environment.SetEnvironmentVariable(EnvVarName, ConnString);

                _sut.EnvironmentVarName = EnvVarName;

                var result = _sut.GetConnectionString();

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

                var result = _sut.GetConnectionString();

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

                Assert.Throws<TestingException>(() => _sut.GetConnectionString());
            }

            [Test]
            public void WhenPathsIsNull_ThenThrowException()
            {
                _sut.FilePaths = null;

                Assert.Throws<TestingException>(() => _sut.GetConnectionString());
            }
        }
    }
}