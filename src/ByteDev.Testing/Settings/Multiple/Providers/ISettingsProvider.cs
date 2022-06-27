namespace ByteDev.Testing.Settings.Multiple.Providers
{
    public interface ISettingsProvider
    {
        TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new();
    }
}