using System;
using ByteDev.Azure.KeyVault.Secrets;
using ByteDev.Testing.Settings.Multiple.Serialization;

namespace ByteDev.Testing.Settings.Multiple.Providers
{
    public class KeyVaultSettingsProvider : ISettingsProvider
    {
        private readonly IKeyVaultSecretClient _kvClient;
        private readonly string _settingPrefix;

        public KeyVaultSettingsProvider(IKeyVaultSecretClient kvClient) : this(kvClient, null)
        {
        }

        public KeyVaultSettingsProvider(IKeyVaultSecretClient kvClient, string settingPrefix)
        {
            _kvClient = kvClient ?? throw new ArgumentNullException(nameof(kvClient));

            _settingPrefix = settingPrefix;
        }

        public TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new()
        {
            try
            {
                return new KeyVaultSettingsSerializer(_kvClient).Deserialize<TTestSettings>(_settingPrefix);
            }
            catch (Exception ex)
            {
                throw new TestingException("Error while trying to deserialize settings object from Azure Key Vault settings.", ex);
            }
        }
    }
}