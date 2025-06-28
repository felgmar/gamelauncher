# gamelauncher

A simple game launcher for Windows, written in Python.

## Features

- Switches Windows power schemes before launching a game
- Restores the original power scheme after the game exits
- Supports launching any executable or shortcut
- Command-line interface

## Installation

```powershell
pip install .
```

Or install from source:

```powershell
git clone https://github.com/felgmar/gamelauncher.git
cd gamelauncher
pip install .
```

## Usage

```powershell
gamelauncher -p "High performance" "C:\Path\To\Game.exe"
```

- If no power scheme is specified, defaults to "Balanced".
- Supports `.lnk` shortcuts.

## Command-line Arguments

- `-p`, `--power-scheme`: Specify the power scheme to use (e.g., "High performance", "Balanced").
- Any additional arguments are passed to the launched process.

## Requirements

- Python 3.10+
- Windows
- [pywin32](https://pypi.org/project/pywin32/)

## License

This project is licensed under the [GNU GPL v3](LICENSE).

---

Feel free to contribute or open issues!
