using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ByteDev.Testing.Settings.Serialization;

namespace ByteDev.Testing.Settings.Providers
{
    /// <summary>
    /// Represents a settings provider for a set of JSON files.
    /// </summary>
    public class JsonFileSettingsProvider : ISettingsProvider
    {
        private IList<string> _filePaths;

        /// <summary>
        /// JSON file paths.
        /// </summary>
        public IList<string> FilePaths
        {
            get => _filePaths ?? (_filePaths = new List<string>());
            set => _filePaths = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Settings.Providers.JsonFileSettingsProvider" /> class.
        /// Based on the containing assembly will attemp to add recommended file paths.
        /// </summary>
        /// <param name="containingAssembly">Containing assembly.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="containingAssembly" /> is null.</exception>
        public JsonFileSettingsProvider(Assembly containingAssembly)
        {
            if (containingAssembly == null)
                throw new ArgumentNullException(nameof(containingAssembly));

            FilePaths = DefaultFilePaths.GetDefaultFilePaths(DefaultFileName.GetJsonSettingsFileName(containingAssembly));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Settings.Providers.JsonFileSettingsProvider" /> class
        /// with the single provided file path.
        /// </summary>
        /// <param name="filePaths">JSON files to add to the list of file paths.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="filePaths" /> is null.</exception>
        public JsonFileSettingsProvider(params string[] filePaths)
        {
            if (filePaths == null)
                throw new ArgumentNullException(nameof(filePaths));

            foreach (var filePath in filePaths)
            {
                FilePaths.Add(filePath);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Settings.Providers.JsonFileSettingsProvider" /> class
        /// with the provided list of file paths.
        /// </summary>
        /// <param name="filePaths">JSON files to add to the list of file paths.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="filePaths" /> is null.</exception>
        public JsonFileSettingsProvider(IList<string> filePaths)
        {
            _filePaths = filePaths ?? throw new ArgumentNullException(nameof(filePaths));
        }
        
        /// <summary>
        /// Attempts to create a new settings instance from the first existing JSON file in the list.
        /// If a new settings instance cannot be created then the type's default will be returned.
        /// </summary>
        /// <typeparam name="TTestSettings">Settings type to create.</typeparam>
        /// <returns>New instance of the settings type.</returns>
        public TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new()
        {
            foreach (var filePath in FilePaths)
            {
                if (File.Exists(filePath))
                    return JsonFileSettingsSerializer.Deserialize<TTestSettings>(filePath);
            }

            return default;
        }
    }
}