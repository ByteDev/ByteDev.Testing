using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace ByteDev.Testing
{
    /// <summary>
    /// Represents test settings that live external to the project in a JSON file.
    /// </summary>
    public class TestSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.TestSettings" /> class.
        /// </summary>
        /// <param name="containingAssembly">Containing test assembly that is consuming the settings.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="containingAssembly" /> is null.</exception>
        public TestSettings(Assembly containingAssembly)
        {
            if (containingAssembly == null)
                throw new ArgumentNullException(nameof(containingAssembly));

            FilePaths = DefaultFilePaths.GetDefaultFilePaths(DefaultFileName.GetDefaultSettingsFileName(containingAssembly));
        }

        /// <summary>
        /// Text file paths that could contain the connection string. By default
        /// will contain a number of possible file paths.
        /// </summary>
        public IEnumerable<string> FilePaths { get; set; }

        /// <summary>
        /// Retrieves the settings from a file deserialized to a given type.
        /// </summary>
        /// <typeparam name="TTestSettings">Type to deserialize to.</typeparam>
        /// <returns>Settings type.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">Could not find test settings file.</exception>
        public TTestSettings GetSettings<TTestSettings>()
        {
            foreach (var filePath in FilePaths)
            {
                if (File.Exists(filePath))
                    return Deserialize<TTestSettings>(filePath);
            }

            throw new TestingException("Could not find test settings file.");
        }

        private static TTestSettings Deserialize<TTestSettings>(string filePath)
        {
            var json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<TTestSettings>(json);
        }
    }
}