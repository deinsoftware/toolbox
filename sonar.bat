@echo off
set /p token=<sonar.txt
:: https://docs.sonarqube.org/display/SONAR/Analysis+Parameters
SonarQube.Scanner.MSBuild.exe begin /k:"dein:toolbox" /n:"ToolBox" /v:"1.2.0" /o:"dein" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="%token%" /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*" /d:sonar.coverage.exclusions="ToolBox.Tests/**,**/*Tests.cs" /d:sonar.cs.opencover.reportsPaths="%cd%\opencover.xml"
dotnet restore
dotnet build
dotnet test ToolBox.Tests/ToolBox.Tests.csproj
OpenCover.Console -target:dotnet.exe -targetdir:"C:\Program Files\dotnet" -targetargs:"test -f netcoreapp2.0 -c Release %cd%\ToolBox.Tests\ToolBox.Tests.csproj" -mergeoutput -hideskipped:File -output:%cd%\opencover.xml -oldStyle -filter:"+[ToolBox*]* -[ToolBox*]*Tests" -searchdirs:%cd%\ToolBox.Tests\bin\Release\netcoreapp2.0 -register:user
SonarQube.Scanner.MSBuild.exe end /d:sonar.login="%token%"