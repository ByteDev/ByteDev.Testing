using System;
using System.Reflection;
using ByteDev.Azure.KeyVault.Secrets;
using ByteDev.Reflection;

namespace ByteDev.Testing.Serialization
{
    internal static class SettingsKeyVaultSerializer
    {
        public static TTestSettings Deserialize<TTestSettings>(Uri keyVaultUri, string settingPrefix) 
            where TTestSettings : class, new()
        {
            var client = new KeyVaultSecretClient(keyVaultUri.AbsoluteUri);

            var settings = new TTestSettings();
            
            var properties = typeof(TTestSettings).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                
            foreach (var pi in properties)
            {
                string kvSettingName = settingPrefix + pi.Name;
                string settingValue = client.GetValueIfExistsAsync(kvSettingName).Result;

                if (settingValue != null)
                    settings.SetPropertyValue(pi, settingValue);
            }

            return settings;
        }
    }
}