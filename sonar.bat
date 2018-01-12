@echo off
set /p token=<sonar.txt
:: https://docs.sonarqube.org/display/SONAR/Analysis+Parameters
SonarQube.Scanner.MSBuild.exe begin /k:"dein:toolbox" /n:"ToolBox" /v:"1.1.2" /o:"dein" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="%token%" /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*"
dotnet restore
dotnet build
dotnet test ToolBox.Tests/ToolBox.Tests.csproj
SonarQube.Scanner.MSBuild.exe end /d:sonar.login="%token%"