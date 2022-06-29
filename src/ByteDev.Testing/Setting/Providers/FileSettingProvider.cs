using System;
using System.Collections.Generic;
using System.IO;

namespace ByteDev.Testing.Setting.Providers
{
    /// <summary>
    /// Represents a setting provider for a set of files.
    /// </summary>
    public class FileSettingProvider : ISettingProvider
    {
        private IList<string> _filePaths;

        /// <summary>
        /// Setting file paths.
        /// </summary>
        public IList<string> FilePaths
        {
            get => _filePaths ?? (_filePaths = new List<string>());
            set => _filePaths = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Setting.Providers.FileSettingProvider" /> class
        /// with the single provided file path.
        /// </summary>
        /// <param name="fileInfo">File to add to the list of file paths.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="fileInfo" /> is null.</exception>
        public FileSettingProvider(FileInfo fileInfo)
        {
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));

            FilePaths.Add(fileInfo.FullName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Setting.Providers.FileSettingProvider" /> class
        /// with the single provided file path.
        /// </summary>
        /// <param name="filePath">File to add to the list of file paths.</param>
        public FileSettingProvider(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path was null or empty.", nameof(filePath));

            FilePaths.Add(filePath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Setting.Providers.FileSettingProvider" /> class
        /// with the provided list of paths.
        /// </summary>
        /// <param name="filePaths">Files to add to the list of file paths.</param>
        public FileSettingProvider(IList<string> filePaths)
        {
            FilePaths = filePaths ?? throw new ArgumentNullException(nameof(filePaths));
        }

        /// <summary>
        /// Attempts to create a new setting string instance from the first existing file in the list.
        /// If a new setting string instance cannot be created then null will be returned.
        /// </summary>
        /// <returns>String setting.</returns>
        public string GetSetting()
        {
            foreach (var filePath in FilePaths)
            {
                if (File.Exists(filePath))
                {
                    var content = File.ReadAllText(filePath).Trim();

                    if (!string.IsNullOrEmpty(content))
                        return content;
                }
            }

            return null;
        }
    }
}