@ECHO off

dotnet build "gamelauncher.sln" --nologo --self-contained --property:OutputPath=%~dp0..\build\ --configuration "Debug"
