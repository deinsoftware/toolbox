#!/bin/bash
token="$(cat sonar.txt)"
# https://docs.sonarqube.org/display/SONAR/Analysis+Parameters
mono ~/Applications/Sonar/Scanner/msbuild/SonarQube.Scanner.MSBuild.exe begin /k:"dein:toolbox" /n:"ToolBox" /v:"1.1.3" /o:"dein" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="${token}" /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*"
dotnet restore
dotnet build
dotnet test ToolBox.Tests/ToolBox.Tests.csproj
mono ~/Applications/Sonar/Scanner/msbuild/SonarQube.Scanner.MSBuild.exe end /d:sonar.login="${token}"