# Change log

<!-- http://keepachangelog.com/en/0.3.0/
Added       for new features.
Changed     for changes in existing functionality.
Deprecated  for once-stable features removed in upcoming releases.
Removed     for deprecated features removed in this release.
Fixed       for any bug fixes.
Security    to invite users to upgrade in case of vulnerabilities.
-->

## 1.11.0 - 2022-12-14

### Added

- Support for .Net version 7.0

### Fixed

- Sponsors url

## 1.10.0 - 2022-05-21

### Changed

- Brand images with new dein standard

## 1.9.2 - 2022-05-19

### Fixed

- Support with dotnet 6 publish NuGet commands

## 1.9.1 - 2022-05-19

### Changed

- Support for Twitter API v2

## 1.9.0 - 2022-03-20

### Removed

- NewtonSoft library dependency

## 1.8.0 - 2022-03-20

### Added

- Support for .Net version 6.0
- Support for netstandard version 2.1

## 1.7.1 - 2020-12-01

### Fixed

- Commands with single quotes and paths with spaces.

## 1.7.0 - 2020-11-11

### Removed

- Drop support for .Net Core version 2.2, because is out of support and will not receive security updates in the future.

### Added

- Support for .Net version 5.0

## 1.6.4 - 2020-04-27

### Fixed

- NuGet development dependency.

## 1.6.3 - 2020-04-22

### Fixed

- NuGet permissions on scripts.

## 1.6.0 - 2020-04-19

### Updated

- The framework to .Net Core version 3.1

### Changed

- NuGet support only for .Net Core 2.2 and 3.1

## 1.5.4 - 2019-10-02

### Updated

- The Framework to .Net Core version 3.0

### Changed

- NuGet support only for .Net Core 2.2 and 3.0

## 1.5.3 - 2019-07-29

### Added

- Path GetFileNameWithoutExtension method

### Updated

- Moq to 4.12.0
- Coverlet to 2.6.3
- NET.Test and CodeCoverage to 16.2.0
- NewtonSoft to 12.0.2

## 1.4.5 - 2019-01-23

### Updated

- The Framework to .Net Core version 2.2
- Moq to 4.10.1
- Coverlet to 4.5.1

### Fixed

- Add extra double quotes on external command when path contains spaces on folder names.

## 1.4.4 - 2018-12-06

### Added

- Support for .Net Core version 2.0

### Changed

- Update Newtonsoft.Json library to 12.0.1

## 1.4.3 - 2018-09-28

### Updated

- The Framework to .Net Core version 2.1

## 1.4.2 - 2018-09-17

### Fixed

- Windows shell bridge.

## 1.4.1 - 2018-08-26

### Fixed

- The `ExtractLine` method on empty search.

## 1.4.0 - 2018-08-25

### Changed

- RemoveLastOctetIPv4 to GetOctetsIPv4 function.

### Added

- Bridge and Shell Documentation.

## 1.3.1 - 2018-08-24

### Fixed

- Output scripts on the NuGet package.

## 1.3.0 - 2018-08-24

### Added

- Bridge class to run commands in an external terminal.

## 1.2.1 - 2018-06-14

### Fixed

- Code smells reported by SonarQube.

### Updated

- Newtonsoft.Json library to 11.0.2

## 1.2.0 - 2018-06-12

### Added

- Search option on Get Files.
- Split function on Directories Path.

## 1.1.4 - 2018-06-07

### Fixed

- Delete All function don't throw an exception when folder file not found.

## 1.1.3 - 2018-02-19

### Fixed

- NuGet package files, remove source files and pack under lib folder.

## 1.1.2 - 2018-01-11

### Fixed

- Code smells reported by SonarQube.

## 1.1.1 - 2018-01-09

### Fixed

- `FilterCreator` function when the extension is null.

## 1.1.0 - 2018-01-05

### Added

- FilterCreator function.
- INotificationSystem interface that notify actions on COPY or DELETE files.
- NuGet package configuration.

### Fixed

- Update [ToolBox](https://github.com/deinsoftware/toolbox) and [Colorify](https://github.com/equiman/colorify) library, that solves bug with text on resize window.

## 1.0.0 - 2017-12-26

Where it all begins...
