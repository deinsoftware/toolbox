# ToolBox [ for Win & Mac ]


**ToolBox** was created to simplify and automate tasks related to NET Core console. Was born in 
[HardHat](https://github.com/equiman/hardhat/) project as a Class. Now grew up as library and can be used by other console applications.

Contributions or Beer :beers: will be appreciated :thumbsup:

> The Code is Dark and Full of Errors!  
> Console is your friend ... don't be afraid!

## Menu

* [Getting Started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [Installing](#installing)
  * [Add Reference](#add-reference)
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

Follow this steps to install on your local machine

Clone **ToolBox** from GitHub on *recommended* path. Using this command on terminal:

| OS | Command |
| --- | --- |
| win | `git clone https://github.com/equiman/toolbox.git "D:\Developer\DEIN\Projects\_devTB"` |
| mac | `git clone https://github.com/equiman/toolbox.git ~/Developer/DEIN/Projects/_devTB` |

## Add Reference

In your project folder, where is located .csproj file run this command on terminal:

| OS | Command |
| --- | --- |
| win | `dotnet add reference "D:\Developer\DEIN\Projects\_devCC\ToolBox\ToolBox.csproj"` |
| mac | `dotnet add reference ~/Developer/DEIN/Projects/_devCC/ToolBox/ToolBox.csproj` |

Take a look on official documentation: [dotnet-add reference](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-reference)

Keep calm, you are almost done. Review this final steps and enjoy the life, no more tedious and repetitive tasks stealing your precious time.

## Usage

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
    private static DiskConfigurator _disk {get; set;}
    private static PathsConfigurator _path {get; set;}

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

#### Disk

```csharp
_disk.CopyAll(source, destination, overwrite, filter[]); //Copy all files and folder from source to destination
_disk.CopyDirectories(source, destination); //Copy all folder from source to destination
_disk.CopyFiles(source, destination, overwrite, filter[]); //Copy all files from source to destination
_disk.DeleteAll(source, recursive); //Delete all files and folders from source
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
                _log = new FileLogCsv(FileSystem.Default);
                break;
            case "txt":
                _log = new FileLogTxt(FileSystem.Default);
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
Env.GetLocalIPv4();           //Return current ip address
Env.RemoveLastOctetIPv4(ip);  //Return ip address with 3 first octets
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

* [VS Code](https://code.visualstudio.com/) - Code editing redefined.

### Contributing

Please read [CONTRIBUTING](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

### Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [Colorify](https://github.com/equiman/colorify/tags) on GitHub.

### Authors

* **Camilo Martinez** [[Equiman](http://stackoverflow.com/story/equiman)]

See also the list of [contributors](https://github.com/equiman/colorify/contributors) who participated in this project.

### License

This project is licensed under the GNU GPLv3 License - see the [LICENSE](LICENSE) file for details.

### Acknowledgments

* [StackOverflow](http://stackoverflow.com): The largest online community for programmers.
* [Dot Net Perls](https://www.dotnetperls.com/console-color): C# Console Color, Text and BackgroundColor.

⇧ [Back to menu](#menu)
