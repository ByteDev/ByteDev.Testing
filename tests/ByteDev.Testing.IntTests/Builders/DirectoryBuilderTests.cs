using System.IO;
using ByteDev.Testing.Builders;
using ByteDev.Testing.NUnit;
using NUnit.Framework;

namespace ByteDev.Testing.IntTests.Builders
{
    [TestFixture]
    [NonParallelizable]
    public class DirectoryBuilderTests : TestBase
    {
        [TestFixture]
        public class Build : DirectoryBuilderTests
        {
            [Test]
            public void WhenDirectoryDoesNotExist_ThenCreate()
            {
                var result = DirectoryBuilder.InFileSystem.WithPath(GetDirectoryName(@"Testing\Test1")).Build();

                AssertDir.Exists(result);
            }

            [Test]
            public void WhenDirectoryExists_AndEmptyTrue_ThenEmpty()
            {
                var dirInfo = DirectoryBuilder.InFileSystem.WithPath(GetDirectoryName("Test2")).Build();

                FileBuilder.InFileSystem.WithPath(Path.Combine(dirInfo.FullName, "DirExists.txt")).Build();

                var result = DirectoryBuilder.InFileSystem
                    .With(dirInfo)
                    .EmptyIfExists()
                    .Build();

                AssertDir.IsEmpty(result);
            }

            [Test]
            public void WhenDirectoryExists_AndEmptyFalse_ThenDontEmpty()
            {
                var dirInfo = DirectoryBuilder.InFileSystem.WithPath(GetDirectoryName("Test3")).Build();

                FileBuilder.InFileSystem.WithPath(Path.Combine(dirInfo.FullName, "DirExists.txt")).Build();

                var result = DirectoryBuilder.InFileSystem
                    .With(dirInfo)
                    .Build();

                AssertDir.ContainsFiles(result, 1);
            }
        }
    }
}