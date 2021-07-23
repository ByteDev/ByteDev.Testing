namespace ByteDev.Testing
{
    /// <summary>
    /// Represents a set of common Azure settings.
    /// </summary>
    public class TestAzureSettings
    {
        /// <summary>
        /// Subscription ID.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Tenant ID.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Client ID.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client secret.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}