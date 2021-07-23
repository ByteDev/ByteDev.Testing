using System.Collections.Generic;

namespace ByteDev.Testing
{
    /// <summary>
    /// Represents a single test setting.
    /// </summary>
    public abstract class TestSingleSetting
    {
        private IList<string> _filePaths;

        /// <summary>
        /// Environment variable name containing the single setting value.
        /// </summary>
        public string EnvironmentVarName { get; set; }

        /// <summary>
        /// Text file paths that could contain the single test setting.
        /// By default will contain a number of possible file paths.
        /// </summary>
        public IList<string> FilePaths
        {
            get => _filePaths ?? (_filePaths = new List<string>());
            set => _filePaths = value;
        }

        /// <summary>
        /// Retrieves the test setting value.
        /// First will try and retrieve from environment variable (process, user, machine).
        /// Second from a text file containing the setting.
        /// </summary>
        /// <returns>Setting value if found; otherwise throws exception.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">Could not find test setting.</exception>
        public abstract string GetValue();

        protected string GetSettingValue(string exceptionMessage)
        {
            var value = SettingEnvironmentReader.GetSingleSetting(EnvironmentVarName);

            if (!string.IsNullOrEmpty(value))
                return value;

            value = SettingFileReader.GetSingleSetting(FilePaths);

            if (!string.IsNullOrEmpty(value))
                return value;

            throw new TestingException(exceptionMessage);
        }
    }
}