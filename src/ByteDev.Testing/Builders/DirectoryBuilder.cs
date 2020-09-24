using System.IO;
using ByteDev.Io;

namespace ByteDev.Testing.Builders
{
    /// <summary>
    /// Represents a builder for directories in the file system.
    /// </summary>
    public class DirectoryBuilder
    {
        private string _dirPath = @"C:\Temp";
        private bool _emptyIfExists;

        private DirectoryBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Builders.DirectoryBuilder" /> class.
        /// </summary>
        public static DirectoryBuilder InFileSystem => new DirectoryBuilder();

        /// <summary>
        /// Directory path to use when creating the directory.
        /// </summary>
        /// <param name="dirInfo">Directory path.</param>
        /// <returns>Current builder instance.</returns>
        public DirectoryBuilder With(DirectoryInfo dirInfo)
        {
            return WithPath(dirInfo.FullName);
        }

        /// <summary>
        /// Directory path to use when creating the directory.
        /// </summary>
        /// <param name="dirPath">Directory path.</param>
        /// <returns>Current builder instance.</returns>
        public DirectoryBuilder WithPath(string dirPath)
        {
            _dirPath = dirPath;
            return this;
        }

        /// <summary>
        /// Indicate whether the directory should be emptied if it
        /// already exists upon creation.
        /// </summary>
        /// <returns></returns>
        public DirectoryBuilder EmptyIfExists()
        {
            _emptyIfExists = true;
            return this;
        }

        /// <summary>
        /// Trys to create the directory in the file system and returns a <see cref="T:System.IO.DirectoryInfo" />
        /// for the directory.
        /// </summary>
        /// <returns><see cref="T:System.IO.DirectoryInfo" /> for the target directory.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">Directory path was not set. A directory path must be provided.</exception>
        public DirectoryInfo Build()
        {
            if (string.IsNullOrEmpty(_dirPath))
                throw new TestingException("Directory path was not set. A directory path must be provided.");

            if (Directory.Exists(_dirPath))
            {
                if (_emptyIfExists)
                {
                    var dirInfo = new DirectoryInfo(_dirPath);
                    dirInfo.EmptyIfExists();
                    return dirInfo;
                }

                return new DirectoryInfo(_dirPath);
            }

            return Directory.CreateDirectory(_dirPath);
        }
    }
}
