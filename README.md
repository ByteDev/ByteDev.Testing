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

Builders for both directories and files.

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

### TestConnectionString

Represents a test connection string that lives external to the test project in possibly either an environment variable or file.

```csharp
// Get reference to assembly that is running the tests
var assembly = Assembly.GetAssembly(typeof(SomeIntTests));

var testConn = new TestConnectionString(assembly);

// Set the name of a possible environment variable where the connection might be held
testConn.EnvironmentVarName = "MyIntTests-ConnString";

// Set possible files where the connection string might be held
testConn.FilePaths = new[]
{
    @"X:\Secure\MyIntTests-ConnString.txt",
    @"C:\User\jonbob\MyIntTests-ConnString.txt",
};

string connStr = testConn.GetConnectionString();
```

---

### TestSettings

Represents test settings that live external to the test project in a JSON file.

```csharp
// Get reference to assembly that is running the tests
var assembly = Assembly.GetAssembly(typeof(SomeIntTests));

var testSettings = new TestSettings(assembly);

// Set possible files where the test settings might be held
testSettings.FilePaths = new[]
{
    @"X:\Secure\MyApp.settings.json",
    @"C:\User\jonbob\MyApp.settings.json",
};

MyAppSettings settings = testSettings.GetSettings<MyAppSettings>();
```

Example settings type:

```csharp
public class MyAppSettings
{
    public string KeyVaultName { get; set; }

    public string ClientId { get; set; }
}
```

Example JSON settings file `MyAppSettings.json` (property name case is ignored):

```json
{
  "KeyVaultName": "my-keyvault",
  "clientId": "98a0d492-c6c6-4f1f-9d19-a98d94242ce6"
}
```

By default property `FilePaths` will be set with a number of default paths based on the assembly name.

For example for the assembly `ByteDev.Testing.IntTests` the defaults are:

```
[0] = "C:\Temp\ByteDev.Testing.IntTests.settings.json"
[1] = "C:\Dev\ByteDev.Testing.IntTests.settings.json"
[2] = "Z:\Dev\ByteDev.Testing.IntTests.settings.json"
[3] = "C:\Users\<user>\ByteDev.Testing.IntTests.settings.json"
[4] = "C:\Users\<user>\Documents\ByteDev.Testing.IntTests.settings.json"
```