using ByteDev.Testing.Builders;
using ByteDev.Testing.NUnit;
using NUnit.Framework;

namespace ByteDev.Testing.IntTests.Builders
{
    [TestFixture]
    [NonParallelizable]
    public class FileBuilderTests : TestBase
    {
        [TestFixture]
        public class Build : FileBuilderTests
        {
            [Test]
            public void WhenNoTextOrSizeSet_ThenWriteEmptyFile()
            {
                var result = FileBuilder.InFileSystem.WithPath(GetFilePath()).Build();

                AssertFile.IsEmpty(result);
            }

            [Test]
            public void WhenTextSet_ThenWriteFile()
            {
                const string content = "some text";

                var result = FileBuilder.InFileSystem
                    .WithPath(GetFilePath())
                    .WithText(content)
                    .Build();

                AssertFile.ContentEquals(result, content);
            }

            [Test]
            public void WhenSizeSet_ThenWriteFile()
            {
                var result = FileBuilder.InFileSystem
                    .WithPath(GetFilePath())
                    .WithSize(10)
                    .Build();

                AssertFile.SizeEquals(result, 10);
            }

            [Test]
            public void WhenFileExists_ThenThrowException()
            {
                const string fileName = "FileBuilder-Test1.txt";

                FileBuilder.InFileSystem.WithPath(GetFilePath(fileName)).Build();

                Assert.Throws<TestingException>(() => FileBuilder.InFileSystem.WithPath(GetFilePath(fileName)).Build());
            }

            [Test]
            public void WhenFileExists_AndAllowOverwrite_ThenWriteFile()
            {
                const string fileName = "FileBuilder-Test2.txt";

                FileBuilder.InFileSystem.WithPath(GetFilePath(fileName)).WithSize(1).Build();
                
                var result = FileBuilder.InFileSystem
                                .WithPath(GetFilePath(fileName))
                                .WithSize(2)
                                .OverwriteIfExists(true)
                                .Build();
                
                AssertFile.SizeEquals(result, 2);
            }
        }
    }
}