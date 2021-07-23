using System;

namespace ByteDev.Testing
{
    internal static class SettingEnvironmentReader
    {
        public static string GetSingleSetting(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;
            
            var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);

            if (string.IsNullOrEmpty(value))
                value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(value))
                value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);

            return value;
        }
    }
}