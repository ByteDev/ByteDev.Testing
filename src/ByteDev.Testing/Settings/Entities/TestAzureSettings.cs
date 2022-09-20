using Azure.Identity;

namespace ByteDev.Testing.Settings.Entities
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

        /// <summary>
        /// Convert to type <see cref="T:Azure.Identity.ClientSecretCredential" />.
        /// </summary>
        /// <returns>New <see cref="T:Azure.Identity.ClientSecretCredential" /> instance.</returns>
        public ClientSecretCredential ToClientSecretCredential()
        {
            return ToClientSecretCredential(null);
        }
        
        /// <summary>
        /// Convert to type <see cref="T:Azure.Identity.ClientSecretCredential" />.
        /// </summary>
        /// <param name="tokenCredentialOptions">Options that allow to configure the management of the requests sent to the Azure Active Directory service.</param>
        /// <returns>New <see cref="T:Azure.Identity.ClientSecretCredential" /> instance.</returns>
        public ClientSecretCredential ToClientSecretCredential(TokenCredentialOptions tokenCredentialOptions)
        {
            return new ClientSecretCredential(
                TenantId, 
                ClientId, 
                ClientSecret,
                tokenCredentialOptions);
        }

        /// <summary>
        /// Convert to type <see cref="T:Azure.Identity.ClientSecretCredential" />.
        /// </summary>
        /// <param name="clientSecretCredentialOptions">Options that allow to configure the management of the requests sent to the Azure Active Directory service.</param>
        /// <returns>New <see cref="T:Azure.Identity.ClientSecretCredential" /> instance.</returns>
        public ClientSecretCredential ToClientSecretCredential(ClientSecretCredentialOptions clientSecretCredentialOptions)
        {
            return new ClientSecretCredential(
                TenantId, 
                ClientId, 
                ClientSecret,
                clientSecretCredentialOptions);
        }
    }
}