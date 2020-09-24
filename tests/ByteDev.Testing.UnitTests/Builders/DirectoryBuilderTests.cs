using ByteDev.Testing.Builders;
using NUnit.Framework;

namespace ByteDev.Testing.UnitTests.Builders
{
    [TestFixture]
    public class DirectoryBuilderTests
    {
        [TestFixture]
        public class Build : DirectoryBuilderTests
        {
            [Test]
            public void WhenPathIsNotSet_ThenThrowException()
            {
                Assert.Throws<TestingException>(() => DirectoryBuilder.InFileSystem.WithPath(null).Build());
            }
        }
    }
}