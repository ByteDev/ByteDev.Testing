using System.Collections.Generic;
using ByteDev.Testing.Settings.Single.Providers;

namespace ByteDev.Testing.Settings.Single
{
    /// <summary>
    /// Represents a single test setting.
    /// </summary>
    public class TestSetting
    {
        private readonly IList<ISettingProvider> _providers = new List<ISettingProvider>();

        /// <summary>
        /// Adds a setting provider to the list of providers.
        /// </summary>
        /// <param name="provider">Provider to add.</param>
        /// <returns>Current test settings instance.</returns>
        public TestSetting AddProvider(ISettingProvider provider)
        {
            _providers.Add(provider);
            return this;
        }

        /// <summary>
        /// Clear all settings providers from the list.
        /// </summary>
        /// <returns>Current test settings instance.</returns>
        public TestSetting ClearProviders()
        {
            _providers.Clear();
            return this;
        }
        
        /// <summary>
        /// Attempts to return a single string setting based on the list of providers.
        /// </summary>
        /// <returns>Single setting as a string.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">No settings providers added or could not create new test setting string.</exception>
        public string GetSetting()
        {
            if (_providers.Count == 0)
                throw new TestingException("No settings providers added. Add at least one.");
            
            foreach (var provider in _providers)
            {
                var setting = provider.GetSetting();

                if (setting != null)
                    return setting;
            }

            throw new TestingException("Could not create new test setting string.");
        }
    }
}