using System.Reflection;
using ByteDev.Azure.KeyVault.Secrets;
using ByteDev.Reflection;

namespace ByteDev.Testing.Settings.Serialization
{
    internal class KeyVaultSettingsSerializer
    {
        private readonly IKeyVaultSecretClient _keyVaultClient;

        public KeyVaultSettingsSerializer(IKeyVaultSecretClient keyVaultClient)
        {
            _keyVaultClient = keyVaultClient;
        }

        public TTestSettings Deserialize<TTestSettings>(string settingPrefix)
            where TTestSettings : class, new()
        {
            var settings = new TTestSettings();
            
            var properties = typeof(TTestSettings).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                
            foreach (var pi in properties)
            {
                string kvSettingName = settingPrefix + pi.Name;
                string settingValue = _keyVaultClient.GetValueIfExistsAsync(kvSettingName).Result;

                if (settingValue != null)
                    settings.SetPropertyValue(pi, settingValue);
            }

            return settings;
        }
    }
}