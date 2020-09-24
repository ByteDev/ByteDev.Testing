using System.IO;

namespace ByteDev.Testing.Builders
{
    /// <summary>
    /// Represents a builder for text files.
    /// </summary>
    public class FileBuilder
    {
        private string _filePath = @"C:\Temp\" + Path.GetRandomFileName();
        private long _size;
        private string _text = string.Empty;
        private bool _overwriteIfExist;

        private FileBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Builders.FileBuilder" /> class
        /// that will make changes in the file system.
        /// </summary>
        public static FileBuilder InFileSystem => new FileBuilder();

        /// <summary>
        /// File path to use when creating the file.
        /// </summary>
        /// <param name="fileInfo">File path.</param>
        /// <returns>Current builder instance.</returns>
        public FileBuilder With(FileInfo fileInfo)
        {
            return WithPath(fileInfo.FullName);
        }

        /// <summary>
        /// File path to use when creating the file.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>Current builder instance.</returns>
        public FileBuilder WithPath(string filePath)
        {
            _filePath = filePath;
            return this;
        }

        /// <summary>
        /// Size in bytes of the file to be created.
        /// </summary>
        /// <param name="size">Size in bytes.</param>
        /// <returns>Current builder instance.</returns>
        public FileBuilder WithSize(long size)
        {
            _size = size < 0 ? 0 : size;
            return this;
        }

        /// <summary>
        /// Text to write to the file to be created.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns>Current builder instance.</returns>
        public FileBuilder WithText(string text)
        {
            _text = text;
            return this;
        }

        /// <summary>
        /// Indicate if should overwrite if the file already exists.
        /// By default this is false.
        /// </summary>
        /// <param name="overwriteIfExist">True overwrite if file exists; otherwise should throw exception.</param>
        /// <returns>Current builder instance.</returns>
        public FileBuilder OverwriteIfExists(bool overwriteIfExist)
        {
            _overwriteIfExist = overwriteIfExist;
            return this;
        }

        /// <summary>
        /// Trys to create the file in the file system and returns a <see cref="T:System.IO.FileInfo" />
        /// for the file.
        /// </summary>
        /// <returns><see cref="T:System.IO.FileInfo" /> for the target file.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">Size cannot be set to >0 and text set to non null/empty.</exception>
        /// <exception cref="T:ByteDev.Testing.TestingException">File path was not set. A file path must be provided.</exception>
        /// <exception cref="T:ByteDev.Testing.TestingException">File already exists.</exception>
        public FileInfo Build()
        {
            if (_size > 0 && !string.IsNullOrEmpty(_text))
                throw new TestingException("Size cannot be set to >0 and text set to non null/empty.");

            if (string.IsNullOrEmpty(_filePath))
                throw new TestingException("File path was not set. A file path must be provided.");

            if (!_overwriteIfExist && File.Exists(_filePath))
                throw new TestingException($"File: '{_filePath}' already exists.");

            using (StreamWriter streamWriter = File.CreateText(_filePath))
            {
                if (!string.IsNullOrEmpty(_text))
                {
                    streamWriter.Write(_text);
                }
                else
                {
                    streamWriter.WriteFillerText(_size);
                }
            }

            return new FileInfo(_filePath);
        }
    }
}