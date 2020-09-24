using ByteDev.Testing.Builders;
using NUnit.Framework;

namespace ByteDev.Testing.UnitTests.Builders
{
    [TestFixture]
    public class FileBuilderTests
    {
        [TestFixture]
        public class Build : FileBuilderTests
        {
            [Test]
            public void WhenSizeSet_AndTextSet_ThenThrowException()
            {
                var sut = FileBuilder.InFileSystem.WithSize(1).WithText("some text");

                Assert.Throws<TestingException>(() => sut.Build());
            }

            [TestCase(null)]
            [TestCase("")]
            public void WhenFilePathNullOrEmpty_ThenThrowException(string filePath)
            {
                var sut = FileBuilder.InFileSystem.WithPath(filePath);

                Assert.Throws<TestingException>(() => sut.Build());
            }
        }
    }
}