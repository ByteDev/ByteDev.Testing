using System;
using System.Collections.Generic;
using System.IO;

namespace ByteDev.Testing.Settings
{
    internal static class DefaultFilePaths
    {
        public static IList<string> GetDefaultFilePaths(string fileName)
        {
            var userName = Environment.UserName;

            var list = new List<string>
            {
                Path.Combine(@"C:\Temp", fileName),
                Path.Combine(@"C:\Dev", fileName),
                Path.Combine(@"Z:\Dev", fileName)
            };

            if (!string.IsNullOrEmpty(userName))
            {
                list.Add(Path.Combine(@"C:\Users\", userName, fileName));
                list.Add(Path.Combine(@"C:\Users\", userName, "Documents", fileName));
            }

            return list;
        }
    }
}