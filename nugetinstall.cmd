:: nuget push IcosLive.Core.1.0.0.nupkg 5E5DE0E::2-D8CB-47BF-AF91-A6D9E066A9FA -source http://icosint:8585/nuget
:: nuget delete IcosLive.Core 1.0.0-alpha -apikey 5E5DE0E2-D8CB-47BF-AF91-A6D9E066A9FA -source http://localhost:8585/nuget
:: nuget setapikey 5E5DE0E2-D8CB-47BF-AF91-A6D9E066A9FA -source http://icosint:8585/nuget

@echo off
setlocal
set cafelibpath=C:\Projects\Eroad\CafeLib
set configuration=Debug
set libPath=bin\%configuration%
set apikey=
set nugetRepo=C:\Nuget\repo
rem set path=%path%;%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\SqlChangeAutomation\OctoPack\build

@echo on
nuget push %cafelibpath%\Core\CafeLib.Core\%libPath%\CafeLib.Core.0.6.1.nupkg %apikey% -source %nugetRepo%
nuget push %cafelibpath%\Core\CafeLib.Core.Binding\%libPath%\CafeLib.Core.Binding.0.6.1.nupkg %apikey% -source %nugetRepo%
nuget push %cafelibpath%\Core\CafeLib.Core.Client.Request\%libPath%\CafeLib.Core.Client.Request.0.6.1.nupkg %apikey% -source %nugetRepo%
nuget push %cafelibpath%\Core\CafeLib.Core.Client.SignalR\%libPath%\CafeLib.Core.Client.SignalR.0.6.1.nupkg %apikey% -source %nugetRepo%
nuget push %cafelibpath%\Core\CafeLib.Core.IoC\%libPath%\CafeLib.Core.IoC.0.6.1.nupkg %apikey% -source %nugetRepo%
nuget push %cafelibpath%\Core\CafeLib.Core.Messaging\%libPath%\CafeLib.Core.Messaging.0.6.1.nupkg %apikey% -source %nugetRepo%
nuget push %cafelibpath%\Core\CafeLib.Core.Queueing\%libPath%\CafeLib.Core.Queueing.0.6.1.nupkg %apikey% -source %nugetRepo%
nuget push %cafelibpath%\Core\CafeLib.Core.Runnable\%libPath%\CafeLib.Core.Runnable.0.6.1.nupkg %apikey% -source %nugetRepo%

rem nuget push %cafelibpath%\Mobile\CafeLib.Mobile\%libPath%\CafeLib.Mobile.0.6.2.nupkg %apikey% -source %nugetRepo%

@echo off

rem nuget push %cafelibpath%\Data\CafeLib.Data.Legacy\%libPath%\IcosLive.Data.Legacy.0.6.8.nupkg %apikey% -source %nugetRepo%
rem nuget push %cafelibpath%\Data\CafeLib.Data.Legacy.Dto\%libPath%\IcosLive.Data.Legacy.Dto.0.6.8.nupkg %apikey% -source %nugetRepo%

endlocal

@echo on
