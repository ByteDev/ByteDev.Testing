# Release Notes

## 5.0.0 - 22 June 2022

Breaking changes:
- Package now targets .NET Standard 2.1.

New features:
- Added functionality to bind settings from Azure Key Vault.

Bug fixes / internal changes:
- Minor XML doc fixes.

## 4.0.0 - 21 March 2022

Breaking changes:
- Added `FakeHttpMessageHandler` class. (Different functionality to `FakeResponseHandler`).
- Removed `FakeResponseHandler` class.

New features:
- Added `FakeRequestOutcome` class.

Bug fixes / internal changes:
- (None)

## 3.1.0 - 11 August 2021

Breaking changes:
- (None)

New features:
- Added `FakeResponseHandler` class.

Bug fixes / internal changes:
- (None)

## 3.0.0 - 23 July 2021

Breaking changes:
- Renamed `TestConnectionString.GetConnectionString` to `GetValue`.

New features:
- Added param less constructor to `TestConnectionString` (option if consumer doesn't want default paths).
- Added param less constructor to `TestSettings` (option if consumer doesn't want default paths).
- Added `TestApiKey` class.
- Added `TestSettings.GetAzureSettings` method.

Bug fixes / internal changes:
- (None)

## 2.0.1 - 29 June 2021

Breaking changes:
- (None)

New features:
- (None)

Bug fixes / internal changes:
- Package fix for dependency.

## 2.0.0 - 29 June 2021

Breaking changes:
- Any error in `TestSettings` while deserializing settings throws a `TestingException` (not `JsonException`).

New features:
- (None)

Bug fixes / internal changes:
- `TestSettings` now deserializes settings JSON file in property name case insensitive manner.
- Updated `ByteDev.Io` dependency to 7.2.1.
- Bug fix to handle when `TestSettings.FilePaths` set to `null`.
- Bug fix to handle when `TestConnectionString.FilePaths` set to `null`.

## 1.1.0 - 20 October 2020

Breaking changes:
- (None)

New features:
- Added `TestAzureSettings`

Bug fixes / internal changes:
- (None)

## 1.0.0 - 24 September 2020

Initial version.
