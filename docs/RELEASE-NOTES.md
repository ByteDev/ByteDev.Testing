# Release Notes

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
