@echo off
:: Bootstrap for the Nuke build. Runs the build project with the installed .NET SDK.
::   build.cmd <Target> [--options]
::   build.cmd --help          list all targets
dotnet run --project "%~dp0nuke\_build.csproj" -- %*
