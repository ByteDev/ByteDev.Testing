using System;

namespace ByteDev.Testing
{
    public class KeyVaultConfig
    {
        public Uri KeyVaultUri { get; set; }

        public string SettingPrefix { get; set; }

        internal bool UseKeyVault => KeyVaultUri != null;
    }
}