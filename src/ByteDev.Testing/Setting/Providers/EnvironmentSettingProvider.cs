using System.Collections.Generic;

namespace ByteDev.Testing.Setting.Providers
{
    /// <summary>
    /// Represents a setting provider for a set of environment variables.
    /// </summary>
    public class EnvironmentSettingProvider : ISettingProvider
    {
        private IList<string> _environmentVarNames;

        /// <summary>
        /// Setting environment variable names.
        /// </summary>
        public IList<string> EnvironmentVarNames
        {
            get => _environmentVarNames ?? (_environmentVarNames = new List<string>());
            set => _environmentVarNames = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Setting.Providers.EnvironmentSettingProvider" /> class
        /// with the single provided environment variable name.
        /// </summary>
        /// <param name="environmentVarName">Environment variable name</param>
        public EnvironmentSettingProvider(string environmentVarName)
        {
            EnvironmentVarNames.Add(environmentVarName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.Setting.Providers.EnvironmentSettingProvider" /> class
        /// with the provided list of environment variables names.
        /// </summary>
        /// <param name="environmentVarNames"></param>
        public EnvironmentSettingProvider(IList<string> environmentVarNames)
        {
            EnvironmentVarNames = environmentVarNames;
        }

        /// <summary>
        /// Attempts to create a new setting string instance from the first existing environment variable 
        /// in the list. If a new setting string instance cannot be created then null will be returned.
        /// </summary>
        /// <returns></returns>
        public string GetSetting()
        {
            foreach (var environmentVarName in EnvironmentVarNames)
            {
                var value = EnvironmentVariableReader.Get(environmentVarName);    

                if (value != null)
                    return value;
            }
            
            return null;
        }
    }
}