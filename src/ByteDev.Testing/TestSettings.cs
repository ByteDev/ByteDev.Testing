﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ByteDev.Testing.Serialization;

namespace ByteDev.Testing
{
    /// <summary>
    /// Represents a set of test settings (from in a JSON file).
    /// </summary>
    public class TestSettings
    {
        private IList<string> _filePaths;
        private KeyVaultConfig _keyVaultConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.TestSettings" /> class.
        /// </summary>
        public TestSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.TestSettings" /> class.
        /// Adds default file paths based on the containing assembly's name.
        /// </summary>
        /// <param name="containingAssembly">Containing test assembly that is consuming the settings.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="containingAssembly" /> is null.</exception>
        public TestSettings(Assembly containingAssembly)
        {
            if (containingAssembly == null)
                throw new ArgumentNullException(nameof(containingAssembly));

            FilePaths = DefaultFilePaths.GetDefaultFilePaths(DefaultFileName.GetJsonSettingsFileName(containingAssembly));
        }

        /// <summary>
        /// JSON file paths that could contain the settings.
        /// </summary>
        public IList<string> FilePaths
        {
            get => _filePaths ?? (_filePaths = new List<string>());
            set => _filePaths = value;
        }

        /// <summary>
        /// Attempts to create the settings first from Azure Key Vault (if KV configuration provied) and
        /// secondly from a JSON settings file.
        /// </summary>
        /// <typeparam name="TTestSettings">Type to deserialize to.</typeparam>
        /// <returns>New instance of the settings type.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">Could not find test settings file or problem while deserializing JSON.</exception>
        public TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new()
        {
            if (KeyVaultConfig.UseKeyVault)
            {
                try
                {
                    return SettingsKeyVaultSerializer.Deserialize<TTestSettings>(KeyVaultConfig);
                }
                catch (Exception ex)
                {
                    throw new TestingException("Error while trying to deserialize settings object from Azure Key Vault settings.", ex);
                }
            }

            foreach (var filePath in FilePaths)
            {
                if (File.Exists(filePath))
                    return SettingsJsonFileSerializer.Deserialize<TTestSettings>(filePath);
            }

            throw new TestingException("Could not create new settings instance.");
        }

        /// <summary>
        /// Retrieves a set of Azure settings from a JSON file.
        /// </summary>
        /// <returns>Azure settings.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">Could not find test settings file or problem while deserializing JSON.</exception>
        public TestAzureSettings GetAzureSettings()
        {
            return GetSettings<TestAzureSettings>();
        }

        public KeyVaultConfig KeyVaultConfig
        {
            get => _keyVaultConfig ?? (_keyVaultConfig = new KeyVaultConfig());
            set => _keyVaultConfig = value;
        }
    }
}