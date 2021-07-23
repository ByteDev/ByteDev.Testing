using System.Reflection;

namespace ByteDev.Testing
{
    internal static class DefaultFileName
    {
        public static string GetConnStringFileName(Assembly assembly)
        {
            return assembly.GetName().Name + ".connstring";
        }

        public static string GetApiKeyFileName(Assembly assembly)
        {
            return assembly.GetName().Name + ".apikey";
        }

        public static string GetJsonSettingsFileName(Assembly assembly)
        {
            return assembly.GetName().Name + ".settings.json";
        }
    }
}