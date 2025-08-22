# gamelauncher

A simple game launcher for Windows, now ported to C# as a .NET console application.

## Overview

- Switches Windows power schemes before launching a game
- Restores the original power scheme after the game exits
- Supports launching any executable or `.lnk` shortcut
- Command-line interface

The repository contains a .NET solution and a `ConsoleApp` project that implements the launcher.

## Build & run (local)

Prerequisite: install the .NET SDK (recommended .NET 9+ to match the project target) and run on Windows.

Build the solution:

```powershell
dotnet build .\gamelauncher.sln -c Release
```

Run directly with the CLI (use from the repo root):

```powershell
dotnet run --project .\ConsoleApp -- -p "High performance" /run "C:\Path\To\Game.exe"
```

If you prefer a single distributable executable, publish a self-contained build for Windows:

```powershell
dotnet publish .\ConsoleApp\ConsoleApp.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
# The resulting exe will be in ConsoleApp\bin\Release\net9.0\win-x64\publish\
```

## Usage

Examples:

```powershell
# Launch an exe with the "High performance" power scheme
dotnet run --project .\ConsoleApp -- -p "High performance" /run "C:\Path\To\Game.exe"

# If you published, run the produced exe directly:
.\ConsoleApp\bin\Release\net9.0\win-x64\publish\ConsoleApp.exe -p "Balanced" "C:\Path\To\Game.lnk"
```

- If no power scheme is specified, the launcher defaults to "Balanced".
- Any additional arguments provided after the target path are forwarded to the launched process.

## Command-line Arguments

- `-p`, `--power-scheme`: Specify the power scheme to use (e.g., "High performance", "Balanced").
- The last positional argument is the path to the executable or shortcut to launch. Any further arguments are passed through to the launched process.

## Requirements

- Windows (the launcher interacts with Windows power schemes and shortcuts)
- .NET SDK 9.0+ to build from source (the project targets `net9.0`)

## Contributing

This repository was recently ported from Python to C#. Development and the console app live in the `ConsoleApp` folder. Small improvements, bug fixes, and tests are welcome — please open issues or pull requests.

## License

This project is licensed under the [GNU GPL v3](LICENSE).

---
