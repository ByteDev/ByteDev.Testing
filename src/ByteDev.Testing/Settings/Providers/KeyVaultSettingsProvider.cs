using System;
using ByteDev.Azure.KeyVault.Secrets;
using ByteDev.Testing.Settings.Serialization;

namespace ByteDev.Testing.Settings.Providers
{
    public class KeyVaultSettingsProvider : ISettingsProvider
    {
        private readonly string _settingPrefix;
        private readonly KeyVaultSettingsSerializer _kvSerializer;

        public KeyVaultSettingsProvider(IKeyVaultSecretClient kvClient) : this(kvClient, null)
        {
        }

        public KeyVaultSettingsProvider(IKeyVaultSecretClient kvClient, string settingPrefix)
        {
            if (kvClient == null)
                throw new ArgumentNullException(nameof(kvClient));

            _kvSerializer = new KeyVaultSettingsSerializer(kvClient);
            _settingPrefix = settingPrefix;
        }

        public TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new()
        {
            try
            {
                return _kvSerializer.DeserializeAsync<TTestSettings>(_settingPrefix).Result;
            }
            catch (Exception ex)
            {
                throw new TestingException("Error while trying to deserialize settings object from Azure Key Vault settings.", ex);
            }
        }
    }
}