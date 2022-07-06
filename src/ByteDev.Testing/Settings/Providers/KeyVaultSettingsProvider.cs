using System;
using ByteDev.Azure.KeyVault.Secrets;
using ByteDev.Azure.KeyVault.Secrets.Serialization;

namespace ByteDev.Testing.Settings.Providers
{
    /// <summary>
    /// Represents a setting provider for settings held as secrets in Azure Key Vault.
    /// </summary>
    public class KeyVaultSettingsProvider : ISettingsProvider
    {
        private readonly string _settingPrefix;
        private readonly IKeyVaultSecretSerializer _kvSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Settings.Providers.KeyVaultSettingsProvider" /> class.
        /// </summary>
        /// <param name="kvClient">Azure Key Vault client used to retrieve the secrets.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="kvClient" /> is null.</exception>
        public KeyVaultSettingsProvider(IKeyVaultSecretClient kvClient) : this(kvClient, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Settings.Providers.KeyVaultSettingsProvider" /> class.
        /// </summary>
        /// <param name="kvClient">Azure Key Vault client used to retrieve the secrets.</param>
        /// <param name="settingPrefix">Prefix to apply to every property name before retrieving from Key Vault.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="kvClient" /> is null.</exception>
        public KeyVaultSettingsProvider(IKeyVaultSecretClient kvClient, string settingPrefix)
        {
            if (kvClient == null)
                throw new ArgumentNullException(nameof(kvClient));

            _kvSerializer = new KeyVaultSecretSerializer(kvClient);
            _settingPrefix = settingPrefix;
        }

        /// <summary>
        /// Attempts to create a new settings instance by binding each public property of the settings type
        /// to a Key Vault secret.
        /// </summary>
        /// <typeparam name="TTestSettings">Settings type to create.</typeparam>
        /// <returns>New instance of the settings type.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">Error while trying to deserialize settings object from Azure Key Vault settings.</exception>
        public TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new()
        {
            try
            {
                var options = new DeserializeOptions
                {
                    SecretNamePrefix = _settingPrefix
                };

                return _kvSerializer.DeserializeAsync<TTestSettings>(options).Result;
            }
            catch (Exception ex)
            {
                throw new TestingException("Error while trying to deserialize settings object from Azure Key Vault settings.", ex);
            }
        }
    }
}