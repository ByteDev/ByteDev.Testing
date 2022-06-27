using System;
using System.Collections.Generic;
using ByteDev.Testing.Settings.Providers;

namespace ByteDev.Testing.Settings
{
    /// <summary>
    /// Represents a set of test settings (from in a JSON file).
    /// </summary>
    public class TestSettings
    {
        private readonly IList<ISettingsProvider> _providers = new List<ISettingsProvider>();

        public TestSettings AddProvider(ISettingsProvider provider)
        {
            _providers.Add(provider);
            return this;
        }

        public TestSettings ClearProviders()
        {
            _providers.Clear();
            return this;
        }

        /// <summary>
        /// Attempts to create a new settings instance based on the given providers.
        /// </summary>
        /// <typeparam name="TTestSettings">Type to deserialize to.</typeparam>
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
        /// Attempts to create a new Azure settings instance based on the given providers.
        /// </summary>
        /// <returns>Azure settings.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">No settings providers added or could not create new test settings instance.</exception>
        public TestAzureSettings GetAzureSettings()
        {
            return GetSettings<TestAzureSettings>();
        }

        /// <summary>
        /// Attempts to create a new Azure Key Vault settings instance based on the given providers.
        /// </summary>
        /// <returns>Azure Key Vault settings.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">No settings providers added or could not create new test settings instance.</exception>
        public TestAzureSettings GetAzureKeyVaultSettings()
        {
            return GetSettings<TestAzureKeyVaultSettings>();
        }
    }
}