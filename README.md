# dein ToolBox [ for Win & Mac ]

[![Build Status](https://travis-ci.org/deinsoftware/toolbox.svg?branch=master)](https://travis-ci.org/deinsoftware/toolbox)
[![NuGet](https://img.shields.io/nuget/v/dein.ToolBox.svg)](https://www.nuget.org/packages/dein.ToolBox/)
[![NuGet](https://img.shields.io/nuget/dt/dein.ToolBox.svg)](https://www.nuget.org/packages/dein.ToolBox/)
[![SonarCloud](https://sonarcloud.io/api/project_badges/measure?project=dein%3Atoolbox&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=dein%3Atoolbox)
[![SonarCloud](https://sonarcloud.io/api/project_badges/measure?project=dein%3Atoolbox&metric=security_rating)](https://sonarcloud.io/dashboard?id=dein%3Atoolbox)
[![SonarCloud](https://sonarcloud.io/api/project_badges/measure?project=dein%3Atoolbox&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=dein%3Atoolbox)
[![SonarCloud](https://sonarcloud.io/api/project_badges/measure?project=dein%3Atoolbox&metric=coverage)](https://sonarcloud.io/dashboard?id=dein%3Atoolbox)

**ToolBox** was created to simplify and automate tasks related to NET Core console. Was born in [HardHat](https://github.com/deinsoftware/hardhat/) project as a Class. Now grew up as library and can be used by other console applications.

Contributions or Beer will be appreciated

> The Code is Dark and Full of Errors!  
> Console is your friend ... don't be afraid!

## Menu

* [Getting Started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [Install](#install)
    * [Dependencies](#dependencies)
    * [Add As Package](#add-as-package)
    * [Add As Reference](#add-as-reference)
  * [Instantiate Library](#instantiate-library)
* [Usage](#usage)
  * [Files](#files)
  * [Log](#log)
  * [Platform](#platform)
  * [System](#system)
  * [Transform](#transform)
  * [Validations](#validations)
* [About](#about)
  * [Built With](#built-with)
  * [Contributing](#contributing)
  * [Versioning](#versioning)
  * [Authors](#authors)
  * [License](#license)
  * [Acknowledgments](#acknowledgments)

---

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

What things you need to install?

* [NET Core SDK](https://www.microsoft.com/net/download)

### Installing

**ToolBox** is available as [project](https://github.com/deinsoftware/toolbox/) or [package](https://www.nuget.org/packages/dein.ToolBox). We strong recommend add as a NuGet package if don't need make modifications directly on source code library.

Follow this instructions to add **ToolBox** in your project.

#### Dependencies

* [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/11.0.2) Library

#### Add As Package

In your project folder, where is located .csproj file run this command on terminal:

```terminal
dotnet add package dein.ToolBox
dotnet add package Newtonsoft.Json --version 11.0.2
```

Official documentation: [dotnet add package](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package)

#### Add As Reference

Clone **ToolBox** from GitHub on *recommended* path. Using this command on terminal:

| OS | Command |
| --- | --- |
| win | `git clone https://github.com/deinsoftware/toolbox.git "D:\Developer\DEIN\Projects\_devTB"` |
| mac | `git clone https://github.com/deinsoftware/toolbox.git ~/Developer/DEIN/Projects/_devTB` |

In your project folder, where is located .csproj file run this command on terminal:

| OS | Command |
| --- | --- |
| win | `dotnet add reference "D:\Developer\DEIN\Projects\_devCC\ToolBox\ToolBox.csproj"` |
| mac | `dotnet add reference ~/Developer/DEIN/Projects/_devCC/ToolBox/ToolBox.csproj` |

Official documentation: [dotnet add reference](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-reference)

⇧ [Back to menu](#menu)

---

## Usage

Keep calm, you are almost done. Review this usage steps and enjoy the life.

### Files

Include operations relative to File System and implements `IFileSystem` and `ICommandSystem` with specific actions and commands per Operative System.

```csharp
using ToolBox.Files;
```

#### Instantiate

On the main class Program, add static properties DiskConfigurator and PathsConfigurator and inside Main method create an instance of the library according the Operative System.

```csharp
class Program
{
    public static DiskConfigurator _disk {get; set;}
    public static PathsConfigurator _path {get; set;}

    static void Main(string[] args)
    {
        _disk = new DiskConfigurator(FileSystem.Default);
        switch (OS.GetCurrent())
        {
            case "win":
                _path = new PathsConfigurator(CommandSystem.Win, FileSystem.Default);
                break;
            case "mac":
                _path = new PathsConfigurator(CommandSystem.Mac, FileSystem.Default);
                break;
        }
        //Foo()
        //Bar()
    }
}
```

If you want to use `_path` and/or `_disk` in other class, add an static using to `Program` class:

```csharp
using static Namesapace.Program;
```

replace Namespace with defined namespace in your project.

#### Disk

```csharp
_disk.FilterCreator(extension[]); //Create a Regex with accepted extensions.
_disk.CopyAll(source, destination, overwrite, filter[]); //Copy all files and folder from source to destination
_disk.CopyDirectories(source, destination); //Copy all folder from source to destination
_disk.CopyFiles(source, destination, overwrite, filter[]); //Copy all files from source to destination
_disk.DeleteAll(source, recursive); //Delete all files and folders from source
```

If you want get Notification about copy or delete process, need implement `INotificationSystem` interface.

```csharp
public sealed class ConsoleNotificationSystem : INotificationSystem
{
    public void ShowAction(string action, string message)
    {
        Console.WriteLine($" [{action}] {message}", txtPrimary);
    }
}
```

And send it as parameter on `DiskConfiguration` definition.

```csharp
_disk = new DiskConfigurator(FileSystem.Default, new ConsoleNotificationSystem());
```

#### Paths

```csharp
_path.Combine(values[]); //Return combined path. Automatic detect `~` as user folder
_path.GetDirectories(path, filter); //Return Folders inside path
_path.GetFiles(path, filter); //Return Files inside path
```

### Log

Include operations relative to Logs and implements `IFileSystem` with specific actions and commands per Operative System.

```csharp
using ToolBox.Log;
```

#### Instantiate

On the main class Program, add static properties ILogSystem and inside Main method create an instance of the class according a value (maybe in your config system).

```csharp
class Program
{
    private static Config _conf { get; set; }
    private static ILogSystem _log {get; set;}

    static void Main(string[] args)
    {
        _conf = Settings.Read();
        switch (_conf.log.system)
        {
            case "csv":
                _log = new FileLogCsv(FileSystem.Default, _path.Combine("~"), ".application.log");
                break;
            case "txt":
                _log = new FileLogTxt(FileSystem.Default, _path.Combine("~"), ".application.log");
                break;
        }
        //Foo()
        //Bar()
    }
}
```

#### Save

```csharp
_log.Save(exception, logLevel); //Save exception on file
```

### Platform

Platform namespace for Operative System detection and commands.

```csharp
using ToolBox.Platform;

OS.IsWin();       //Return true on Windows
OS.IsMac();       //Return true on MacOS
OS.IsGnu();       //Return true on Linux

OS.GetCurrent();
//Return "win" on Windows
//Return "mac" on MacOS
//Return "gnu" on Linux
```

### System

Include operations relative to System.

```csharp
using ToolBox.System;
```

#### Environment

```csharp
Env.GetValue(key);        //Return value from key
Env.SetValue(key, value); //Set value to key (only for program session, not permanent)
Env.IsNullOrEmpty(key);   //Return true when value from key is defined
```

#### Network

```csharp
Network.GetLocalIPv4();           //Return current ip address
Network.RemoveLastOctetIPv4(ip);  //Return ip address with 3 first octets
```

#### User

```csharp
User.GetUserName(); //Return logged username
User.GetMachine();  //Return machine name
User.GetDomain();   //Return domain name
```

### Transform

Include operations relative to Transform Text.

```csharp
using ToolBox.Transform;
```

#### Strings

```csharp
Strings.RemoveWords(oldValue, wordToRemove[]);  //Return oldValue without removed words
```

### Validations

Include operations relative to Validations.

```csharp
using ToolBox.Validations;
```

#### Boolean

```csharp
Bool.SomeFalse(values[]);  //Return true if there is false value
```

#### Number

```csharp
Number.IsNumber(value);             //Return true if value is number
Number.IsOnRange(min, value, max);  //Return true if number is between min and max
```

#### Strings

```csharp
Strings.SomeNullOrEmpty(values[]);  //Return true if there is an empty or null value
```

#### Web

```csharp
Web.IsUrl(value); //Return true if value is an http or https valid address
```

⇧ [Back to menu](#menu)

---

## About

### Built With

* [.Net Core](https://dotnet.github.io/) - ASP.NET Core is a free and open-source web framework, and the next generation of ASP.NET, developed by Microsoft and the community.
* [VS Code](https://code.visualstudio.com/) - Code editing redefined.
* [SonarQube](https://sonarcloud.io/dashboard/index/dein:toolbox) - Continuous code quality.

### Contributing

Please read [CONTRIBUTING](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

### Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [ToolBox](https://github.com/deinsoftware/toolbox/tags) on GitHub.

### Authors

* **Camilo Martinez** [[Equiman](http://stackoverflow.com/story/equiman)]

See also the list of [contributors](https://github.com/deinsoftware/toolbox/contributors) who participated in this project.

### License

This project is licensed under the GNU GPLv3 License - see the [LICENSE](LICENSE) file for details.

### Acknowledgments

* [StackOverflow](http://stackoverflow.com): The largest online community for programmers.
* [Dot Net Perls](https://www.dotnetperls.com/console-color): C# Console Color, Text and BackgroundColor.

⇧ [Back to menu](#menu)
