using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<TTestSettings> DeserializeAsync<TTestSettings>(string settingPrefix, CancellationToken cancellationToken = default)
            where TTestSettings : class, new()
        {
            var properties = typeof(TTestSettings).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            var propertyNames = properties.Select(s => s.Name).ToList();
            var kvNames = properties.Select(s => settingPrefix + s.Name);

            var dictionary = await _keyVaultClient.GetValuesIfExistsAsync(kvNames, false, cancellationToken);
            
            var settings = new TTestSettings();

            int index = 0;

            foreach (var item in dictionary)
            {
                if (item.Value != null)
                    settings.SetPropertyValue(propertyNames[index], item.Value);

                index++;
            }

            return settings;
        }
    }
}