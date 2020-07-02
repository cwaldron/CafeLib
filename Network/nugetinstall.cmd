@echo off
setlocal

:: Configuration
set lib=Network
set version=0.7.5

:: Settings
set nuget=nuget.exe
::set nuget=dotnet nuget
set configuration=Debug
set libPath=bin\%configuration%
set apikey=
set nugetRepo=C:\Nuget\repo
set sourcepath=C:\Projects\ChrisSolutions\CafeLib\%lib%

@echo on
%nuget% push %sourcepath%\CafeLib.Web.Request\%libPath%\CafeLib.Web.Request.%version%.nupkg %apikey% -source %nugetRepo%

@echo off

endlocal