namespace ByteDev.Testing.Settings.Entities
{
    /// <summary>
    /// Represents settings for a Azure key vault service.
    /// </summary>
    public class TestAzureKeyVaultSettings : TestAzureSettings
    {
        /// <summary>
        /// Azure key vault service name.
        /// </summary>
        public string KeyVaultName { get; set; }

        /// <summary>
        /// URI to the Azure key vault service.
        /// </summary>
        public string KeyVaultUri { get; set; }
    }
}