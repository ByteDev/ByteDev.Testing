using System;
using System.Reflection;

namespace ByteDev.Testing.Settings
{
    /// <summary>
    /// Represents a test connection string setting.
    /// </summary>
    public class TestConnectionString : TestSingleSetting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Settings.TestConnectionString" /> class.
        /// </summary>
        public TestConnectionString()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Settings.TestConnectionString" /> class.
        /// Adds default file paths based on the containing assembly's name.
        /// </summary>
        /// <param name="containingAssembly">Containing test assembly that is consuming the setting.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="containingAssembly" /> is null.</exception>
        public TestConnectionString(Assembly containingAssembly)
        {
            if (containingAssembly == null)
                throw new ArgumentNullException(nameof(containingAssembly));

            FilePaths = DefaultFilePaths.GetDefaultFilePaths(DefaultFileName.GetConnStringFileName(containingAssembly));
        }
        
        public override string GetValue()
        {
            return GetSettingValue("Could not find test connection string.");
        }
    }
}