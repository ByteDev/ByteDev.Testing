using System;
using System.Collections.Generic;
using ByteDev.Testing.Settings.Multiple.Entities;
using ByteDev.Testing.Settings.Multiple.Providers;

namespace ByteDev.Testing.Settings.Multiple
{
    /// <summary>
    /// Represents a set of test settings.
    /// </summary>
    public class TestSettings
    {
        private readonly IList<ISettingsProvider> _providers = new List<ISettingsProvider>();

        /// <summary>
        /// Adds a settings provider to the list of providers.
        /// </summary>
        /// <param name="provider">Provider to add.</param>
        /// <returns>Current test settings instance.</returns>
        public TestSettings AddProvider(ISettingsProvider provider)
        {
            _providers.Add(provider);
            return this;
        }

        /// <summary>
        /// Clear all settings providers from the list.
        /// </summary>
        /// <returns>Current test settings instance.</returns>
        public TestSettings ClearProviders()
        {
            _providers.Clear();
            return this;
        }

        /// <summary>
        /// Attempts to create a new settings instance based on the list of providers.
        /// </summary>
        /// <typeparam name="TTestSettings">Settings type to create.</typeparam>
        /// <returns>New instance of the settings type.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">No settings providers added or could not create new test settings instance.</exception>
        public TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new()
        {
            if (_providers.Count == 0)
                throw new TestingException("No settings providers added. Add at least one.");

            Exception lastProviderEx = null;

            foreach (var provider in _providers)
            {
                try
                {
                    var settings = provider.GetSettings<TTestSettings>();

                    if (settings != null)
                        return settings;
                }
                catch (Exception ex)
                {
                    lastProviderEx = ex;
                }
            }

            throw new TestingException("Could not create new test settings instance.", lastProviderEx);
        }

        /// <summary>
        /// Attempts to create a new Azure settings instance based on the list of providers.
        /// </summary>
        /// <returns>Azure settings.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">No settings providers added or could not create new test settings instance.</exception>
        public TestAzureSettings GetAzureSettings()
        {
            return GetSettings<TestAzureSettings>();
        }

        /// <summary>
        /// Attempts to create a new Azure Key Vault settings instance based on the list of providers.
        /// </summary>
        /// <returns>Azure Key Vault settings.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">No settings providers added or could not create new test settings instance.</exception>
        public TestAzureSettings GetAzureKeyVaultSettings()
        {
            return GetSettings<TestAzureKeyVaultSettings>();
        }
    }
}