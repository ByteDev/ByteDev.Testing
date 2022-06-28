using System;
using System.Collections.Generic;
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

            var tasks = new List<Task<string>>();
            var nameValues = new List<Tuple<string, Task<string>>>();

            foreach (var pi in properties)
            {
                string kvSettingName = settingPrefix + pi.Name;
                
                var task = _keyVaultClient.GetValueIfExistsAsync(kvSettingName, cancellationToken);

                tasks.Add(task);
                nameValues.Add(new Tuple<string, Task<string>>(pi.Name, task));
            }

            await Task.WhenAll(tasks);

            return CreateTestSettings<TTestSettings>(nameValues);
        }

        private static TTestSettings CreateTestSettings<TTestSettings>(IEnumerable<Tuple<string, Task<string>>> nameValues) where TTestSettings : class, new()
        {
            var settings = new TTestSettings();

            foreach (var nameValue in nameValues)
            {
                if (nameValue.Item2.Result != null)
                    settings.SetPropertyValue(nameValue.Item1, nameValue.Item2.Result);
            }

            return settings;
        }
    }
}