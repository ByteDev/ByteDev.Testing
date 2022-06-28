using System;
using System.Collections.Generic;
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

            var nameTasks = new List<Tuple<string, Task<string>>>();

            foreach (var pi in properties)
            {
                string kvSettingName = settingPrefix + pi.Name;
                
                var task = _keyVaultClient.GetValueIfExistsAsync(kvSettingName, cancellationToken);

                nameTasks.Add(new Tuple<string, Task<string>>(pi.Name, task));
            }

            IEnumerable<Task<string>> tasks = nameTasks.Select(i => i.Item2);

            await Task.WhenAll(tasks);

            return CreateTestSettings<TTestSettings>(nameTasks);
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