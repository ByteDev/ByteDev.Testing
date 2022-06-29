namespace ByteDev.Testing.Settings.Providers
{
    /// <summary>
    /// Represents the interface for a settings provider.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Attempts to create a new settings instance.
        /// </summary>
        /// <typeparam name="TTestSettings">Settings type to create.</typeparam>
        /// <returns>New instance of the settings type.</returns>
        TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new();
    }
}