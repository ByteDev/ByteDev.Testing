using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ByteDev.Testing.Settings.Serialization;

namespace ByteDev.Testing.Settings.Providers
{
    public class JsonFileSettingsProvider : ISettingsProvider
    {
        private IList<string> _filePaths;

        public IList<string> FilePaths
        {
            get => _filePaths ?? (_filePaths = new List<string>());
            set => _filePaths = value;
        }

        public JsonFileSettingsProvider(Assembly containingAssembly)
        {
            if (containingAssembly == null)
                throw new ArgumentNullException(nameof(containingAssembly));

            FilePaths = DefaultFilePaths.GetDefaultFilePaths(DefaultFileName.GetJsonSettingsFileName(containingAssembly));
        }

        public JsonFileSettingsProvider(IList<string> filePaths)
        {
            _filePaths = filePaths;
        }
        
        public TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new()
        {
            foreach (var filePath in FilePaths)
            {
                if (File.Exists(filePath))
                    return JsonFileSettingsSerializer.Deserialize<TTestSettings>(filePath);
            }

            return default;
        }
    }
}