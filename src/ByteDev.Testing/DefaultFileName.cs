using System.Reflection;

namespace ByteDev.Testing
{
    internal static class DefaultFileName
    {
        public static string GetDefaultConnStringFileName()
        {
            return GetDefaultConnStringFileName(GetThisAssembly());
        }

        public static string GetDefaultConnStringFileName(Assembly assembly)
        {
            return assembly.GetName().Name + ".connstring";
        }

        public static string GetDefaultSettingsFileName()
        {
            return GetDefaultSettingsFileName(GetThisAssembly());
        }
        
        public static string GetDefaultSettingsFileName(Assembly assembly)
        {
            return assembly.GetName().Name + ".settings.json";
        }

        private static Assembly GetThisAssembly()
        {
            var assembly = Assembly.GetAssembly(typeof(TestConnectionString));

            if (assembly == null)
                throw new TestingException("Cannot find containing assembly.");

            return assembly;
        }
    }
}