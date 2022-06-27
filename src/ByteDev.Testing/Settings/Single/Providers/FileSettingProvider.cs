using System.Collections.Generic;
using System.IO;

namespace ByteDev.Testing.Settings.Single.Providers
{
    public class FileSettingProvider : ISettingProvider
    {
        private IList<string> _filePaths;

        public IList<string> FilePaths
        {
            get => _filePaths ?? (_filePaths = new List<string>());
            set => _filePaths = value;
        }

        public FileSettingProvider(IList<string> filePaths)
        {
            FilePaths = filePaths;
        }

        public string GetSetting()
        {
            foreach (var filePath in FilePaths)
            {
                if (File.Exists(filePath))
                    return File.ReadAllText(filePath).Trim();
            }

            return null;
        }
    }
}