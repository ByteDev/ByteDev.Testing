using System.Collections.Generic;
using System.IO;

namespace ByteDev.Testing
{
    internal static class SettingFileReader
    {
        public static string GetSingleSetting(IEnumerable<string> filePaths)
        {
            if (filePaths == null)
                return null;

            foreach (var filePath in filePaths)
            {
                if (File.Exists(filePath))
                    return File.ReadAllText(filePath).Trim();
            }

            return null;
        }
    }
}