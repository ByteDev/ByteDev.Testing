namespace ByteDev.Testing.Providers
{
    public interface ISettingsProvider
    {
        TTestSettings GetSettings<TTestSettings>() where TTestSettings : class, new();
    }
}