[![Build status](https://ci.appveyor.com/api/projects/status/github/bytedev/ByteDev.Testing?branch=master&svg=true)](https://ci.appveyor.com/project/bytedev/ByteDev-Testing/branch/master)
[![NuGet Package](https://img.shields.io/nuget/v/ByteDev.Testing.svg)](https://www.nuget.org/packages/ByteDev.Testing)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/ByteDev/ByteDev.Testing/blob/master/LICENSE)

# ByteDev.Testing

.NET Standard library that provides some testing related functionality.

## Installation

ByteDev.Testing has been written as a .NET Standard 2.0 library, so you can consume it from a .NET Core or .NET Framework 4.6.1 (or greater) application.

ByteDev.Testing is hosted as a package on nuget.org.  To install from the Package Manager Console in Visual Studio run:

`Install-Package ByteDev.Testing`

Further details can be found on the [nuget page](https://www.nuget.org/packages/ByteDev.Testing/).

## Release Notes

Releases follow semantic versioning.

Full details of the release notes can be viewed on [GitHub](https://github.com/ByteDev/ByteDev.Testing/blob/master/docs/RELEASE-NOTES.md).

## Usage

The library is split into the following main parts:
- Builders
- HTTP
- Settings

---

### Builders - DirectoryBuilder

`DirectoryBuilder` helps to create directories quickly for testing purposes.

```csharp
using ByteDev.Testing.Builders;

// ...

// Create a directory quickly on disk
DirectoryInfo dir = DirectoryBuilder.InFileSystem
                        .WithPath(@"C:\Temp\Testing")
                        .EmptyIfExists()
                        .Build();
```

---

### Builders - FileBuilder

`FileBuilder` helps to create files quickly for testing purposes.

```csharp
using ByteDev.Testing.Builders;

// ...

// Create a file quickly on disk
FileInfo file = FileBuilder.InFileSystem
                    .WithPath(@"C:\Temp\Testing\TestFile1.txt")
                    .WithSize(10)
                    .OverwriteIfExists(true)
                    .Build();
```

---

### HTTP - FakeHttpMessageHandler

`FakeHttpMessageHandler` allows you to provide a sequence of *outcomes* (HTTP responses or exceptions thrown) that correspond to each HTTP request made.

```csharp
using ByteDev.Testing.Http;

// ...

var uri = new Uri("http://www.google.com/");

// Create a sequence of outcomes...
// 1st request will return a response OK with content.
// 2nd request will throw request timeout exception.
var outcomes = new List<FakeRequestOutcome>
{
    new FakeRequestOutcome(new HttpResponseMessage(HttpStatusCode.OK)
    {
        Content = new StringContent("Testing")
    }),
    new FakeRequestOutcome(FakeRequestOutcome.CreateRequestTimeout())
}

// Create a fake HTTP handler
var handler = new FakeHttpMessageHandler(outcomes);

var httpClient = new HttpClient(handler);

var response1 = await httpClient.GetAsync(uri);
var content = await response1.Content.ReadAsStringAsync();

// response1.StatusCode == HttpStatusCode.OK
// content == "Testing"

// Throws TaskCanceledException("Request has timed out.")
var response2 = await httpClient.GetAsync(uri);

// handler.RequestsMade == 2
```

---

### Settings - TestSetting

The `TestSetting` type represents a single string value setting that lives external to a test project.

```csharp
using ByteDev.Testing.Settings.Single;
using ByteDev.Testing.Settings.Single.Providers;

// ...

var testSetting = new TestSetting();

testSetting.AddProvider(new FileSettingProvider(new[]
{
    "C:\Dev\SomeSetting.apikey",
    "C:\Temp\AnotherSetting.txt"
}));

testSetting.AddProvider(new EnvironmentSettingProvider("MyProjEnvVar"));

string setting = testSetting.GetSetting();

// The two files will be checked first for the setting and if 
// not found the environment variable will be checked.
```

---

### Setting TestSettings

The `TestSettings` type represents sets of test settings that live external to a test project.

Example custom settings type:

```csharp
public class MyAppSettings
{
    public string MySecret1 { get; set; }

    public string MySecret2 { get; set; }
}
```

Example JSON settings file `MyApp.settings.json` (property name case is ignored):

```json
{
  "MySecret1": "some secret 1",
  "MySecret2": "some secret 2"
}
```

```csharp
using ByteDev.Azure.KeyVault.Secrets;
using ByteDev.Testing.Settings.Multiple;
using ByteDev.Testing.Settings.Multiple.Providers;

// ...
var kvClient = new KeyVaultSecretClient(keyVaultUri);

var testSettings = new TestSettings();

testSettings
    .AddProvider(new JsonFileSettingsProvider(@"X:\Secure\MyApp.settings.json"))
    .AddProvider(new KeyVaultSettingsProvider(kvClient));

// The TestSettings type will try to get the settings from the JSON
// file first and if it fails will try Azure Key Vault

MyAppSettings settings = testSettings.GetSettings<MyAppSettings>();
```

As well as defining and providing your own settings type you can also use the built in `TestAzureSettings` and `TestAzureKeyVaultSettings` classes. 

For example:

```csharp
using ByteDev.Testing.Settings.Multiple.Entities;

// ...

TestAzureSettings settings = testSettings.GetAzureSettings();

// TestAzureSettings contains common Azure settings, including:
// SubscriptionId, TenantId, ClientId, ClientSecret etc.
```

---


