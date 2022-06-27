using System;
using System.Reflection;

namespace ByteDev.Testing.Settings
{
    /// <summary>
    /// Represents a test API key setting.
    /// </summary>
    public class TestApiKey : TestSingleSetting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Settings.TestApiKey" /> class.
        /// </summary>
        public TestApiKey()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Settings.TestApiKey" /> class.
        /// Adds default file paths based on the containing assembly's name.
        /// </summary>
        /// <param name="containingAssembly">Containing test assembly that is consuming the setting.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="containingAssembly" /> is null.</exception>
        public TestApiKey(Assembly containingAssembly)
        {
            if (containingAssembly == null)
                throw new ArgumentNullException(nameof(containingAssembly));

            FilePaths = DefaultFilePaths.GetDefaultFilePaths(DefaultFileName.GetApiKeyFileName(containingAssembly));
        }

        public override string GetValue()
        {
            return GetSettingValue("Could not find test API key.");
        }
    }
}