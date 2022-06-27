using System.IO;
using System.Text.Json;

namespace ByteDev.Testing.Settings.Multiple.Serialization
{
    internal static class JsonFileSettingsSerializer
    {
        public static TTestSettings Deserialize<TTestSettings>(string filePath)
        {
            var json = File.ReadAllText(filePath);

            try
            {
                return JsonSerializer.Deserialize<TTestSettings>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                throw new TestingException($"Error while deserializing JSON settings in file: '{filePath}'. Check JSON is valid.", ex);
            }
        }
    }
}