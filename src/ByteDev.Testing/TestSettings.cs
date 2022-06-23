using System;
using System.Collections.Generic;
using ByteDev.Testing.Providers;

namespace ByteDev.Testing
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
        /// Attempts to create the settings first from Azure Key Vault (if KV configuration provied) and
        /// secondly from a JSON settings file.
        /// </summary>
        /// <typeparam name="TTestSettings">Type to deserialize to.</typeparam>
        /// <returns>New instance of the settings type.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">Could not find test settings file or problem while deserializing JSON.</exception>
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
        /// Retrieves a set of Azure settings from a JSON file.
        /// </summary>
        /// <returns>Azure settings.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">Could not find test settings file or problem while deserializing JSON.</exception>
        public TestAzureSettings GetAzureSettings()
        {
            return GetSettings<TestAzureSettings>();
        }
    }
}