@ECHO off

dotnet publish "gamelauncher.sln" --nologo --self-contained --property:OutputPath=%~dp0..\build\ --configuration "Release"
