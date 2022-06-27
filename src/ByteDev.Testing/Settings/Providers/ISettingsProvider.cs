namespace ByteDev.Testing.Settings.Providers
{
    public interface ISettingsProvider
    {
        TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new();
    }
}