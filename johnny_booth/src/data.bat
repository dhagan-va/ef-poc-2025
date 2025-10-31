SET MIGRATION=%2
IF "%MIGRATION%"=="" SET MIGRATION=InitialCreate

goto %1

:migrate
echo Migrating database
dotnet ef migrations add %MIGRATION% --project X12EDI.Data --startup-project X12EDI.CLI
goto :eof

:installef
dotnet tool install --global dotnet-ef
goto :eof

:update
echo Database update
dotnet ef database update --project X12EDI.Data --startup-project X12EDI.CLI
goto :eof

:help
set scriptname=data.bat
echo Usage:
echo   %scriptname% migrate [MigrationName]
echo   %scriptname% update
echo   %scriptname% installef
goto :eof
