using System.Collections.Generic;

namespace ByteDev.Testing.Settings.Single.Providers
{
    public class EnvironmentSettingProvider : ISettingProvider
    {
        private IList<string> _environmentVarNames;

        public IList<string> EnvironmentVarNames
        {
            get => _environmentVarNames ?? (_environmentVarNames = new List<string>());
            set => _environmentVarNames = value;
        }
        
        public EnvironmentSettingProvider(IList<string> environmentVarNames)
        {
            EnvironmentVarNames = environmentVarNames;
        }

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