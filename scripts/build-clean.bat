@ECHO off

CALL %~dp0/cleanup.bat

dotnet build "gamelauncher.sln" --nologo --self-contained --property:OutputPath=%~dp0..\build\ --configuration "Debug"
