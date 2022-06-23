using System;
using ByteDev.Testing.Serialization;

namespace ByteDev.Testing.Providers
{
    public class KeyVaultSettingsProvider : ISettingsProvider
    {
        public Uri KeyVaultUri { get; }

        public string SettingPrefix { get; }

        public KeyVaultSettingsProvider(Uri keyVaultUri) : this(keyVaultUri, null)
        {
        }

        public KeyVaultSettingsProvider(Uri keyVaultUri, string settingPrefix)
        {
            if (keyVaultUri == null)
                throw new ArgumentNullException(nameof(keyVaultUri));

            KeyVaultUri = keyVaultUri;
            SettingPrefix = settingPrefix;
        }

        public TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new()
        {
            try
            {
                return SettingsKeyVaultSerializer.Deserialize<TTestSettings>(KeyVaultUri, SettingPrefix);
            }
            catch (Exception ex)
            {
                throw new TestingException("Error while trying to deserialize settings object from Azure Key Vault settings.", ex);
            }
        }
    }
}