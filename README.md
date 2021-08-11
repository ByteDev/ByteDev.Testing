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

### Builders

Builders to help create both directories and files quickly for testing purposes.

```csharp
// Create a directory quickly on disk
DirectoryInfo dir = DirectoryBuilder.InFileSystem
                        .WithPath(@"C:\Temp\Testing")
                        .EmptyIfExists()
                        .Build();


// Create a file quickly on disk
FileInfo file = FileBuilder.InFileSystem
                    .WithPath(@"C:\Temp\Testing\TestFile1.txt")
                    .WithSize(10)
                    .OverwriteIfExists(true)
                    .Build();
```

---

### TestConnectionString & TestApiKey

The `TestConnectionString` and `TestApiKey` types both represent different types of single value string settings that live external to the test project.

These settings can be stored possibly in either an environment variable or file.

Example usage of `TestConnectionString`:

```csharp
// Get reference to assembly that is running the tests
var assembly = Assembly.GetAssembly(typeof(SomeIntTests));

// Create an instance of the type. A number of default file paths will be set on FilePath
// when a reference to the containing assembly is passed on the constructor.
var testConn = new TestConnectionString(assembly);

// Set the name of a possible environment variable where the connection might be held
testConn.EnvironmentVarName = "MyIntTests-ConnString";

// Set extra file paths where the connection string might be held.
testConn.FilePaths.Add(@"X:\Secure\MyIntTests.connstring");

string connStr = testConn.GetValue();
```

---

### TestSettings

The `TestSettings` type represents sets of test settings that live external to the test project in a JSON file.

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

Example code to retrie `MyAppSettings`:

```csharp
// Get reference to assembly that is running the tests
var assembly = Assembly.GetAssembly(typeof(SomeIntTests));

// Create an instance of the type. A number of default file paths will be set on FilePath
// when a reference to the containing assembly is passed on the constructor.
var testSettings = new TestSettings(assembly);

// // Set extra file paths where the test settings might be held
testSettings.FilePaths.Add(@"X:\Secure\MyApp.settings.json");

MyAppSettings settings = testSettings.GetSettings<MyAppSettings>();

// settings.MySecret1 == "some secret 1"
// settings.MySecret2 == "some secret 2"
```

As well as defining and providing your own settings type you can also use the built in `TestAzureSettings` type. 

For example:

```csharp
TestAzureSettings settings = testSettings.GetAzureSettings();

// TestAzureSettings contains common Azure settings, including:
// SubscriptionId, TenantId, ClientId, ClientSecret etc.
```

---

### FakeResponseHandler

Allows you to provide a fake HTTP status code and content to return from a `HttpClient` call.

Reference in the `ByteDev.Testing.Http` namespace.

```csharp
var code = HttpStatusCode.OK;
var content = new StringContent("test");

var client = new HttpClient(new FakeResponseHandler(code, content));

var response = await client.GetAsync("http://www.google.com/");

var str = await await result.Content.ReadAsStringAsync();

// response.StatusCode == HttpStatusCode.OK
// str == "test"
```
