@echo off

:: https://docs.sonarqube.org/display/SONAR/Analysis+Parameters
mono D:\Applications\Sonar\Scanner\msbuild\SonarQube.Scanner.MSBuild.exe begin /k:"dein:toolbox" /n:"ToolBox" /v:"1.1.1" /o:"dein" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="34dadb57c7435b016b8ba12e48dc49f5d1eadee6" /d:sonar.language=cs /d:sonar.exclusions="**/bin/**/*,**/obj/**/*"
dotnet restore
dotnet build
dotnet test ToolBox.Tests/ToolBox.Tests.csproj
mono D:\Applications\Sonar\Scanner\msbuild\SonarQube.Scanner.MSBuild.exe end /d:sonar.login="34dadb57c7435b016b8ba12e48dc49f5d1eadee6"