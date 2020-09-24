using System.IO;
using ByteDev.Io;
using ByteDev.Testing.Builders;
using NUnit.Framework;

namespace ByteDev.Testing.IntTests
{
    [TestFixture]
    public abstract class TestBase
    {
        protected const string ExistingDirectoryPath = @"C:\Temp\ByteDev.Testing.IntTests";

        protected readonly DirectoryInfo ExistingDirectory = new DirectoryInfo(ExistingDirectoryPath);

        [SetUp]
        public void SetUp()
        {   
            ExistingDirectory.DeleteIfExists();
            ExistingDirectory.Create();
        }

        [TearDown]
        public void TearDown()
        {
            ExistingDirectory.DeleteIfExists();
        }

        protected string GetFilePath()
        {
            return GetFilePath(Path.GetRandomFileName());
        }

        protected string GetFilePath(string fileName)
        {
            return Path.Combine(ExistingDirectoryPath, fileName);
        }

        protected string GetDirectoryName(string dirName)
        {
            return Path.Combine(ExistingDirectoryPath, dirName);
        }

        protected FileInfo CreateTextFile(string content)
        {
            return FileBuilder.InFileSystem
                .WithPath(GetFilePath())
                .WithText(content)
                .Build();
        }
    }
}