"""
This modules manages power plan stuff.
"""

if __name__ == "__main__":
    raise ImportError("This module cannot be used directly.")

import re
import subprocess

from gamelauncher import process, regex

def __get_active_power_scheme():
    """
    Get the active power scheme on Windows.
    Returns:
        str: The active power scheme GUID.
    """
    active_power_scheme: dict[str, str] = {}

    GUID_PATTERN = regex.get_guid_pattern()

    try:
        result: subprocess.CompletedProcess[str] = subprocess.run(
            ['powercfg', '/getactivescheme'],
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            text=True,
            check=True,
            creationflags=subprocess.CREATE_NO_WINDOW
        )

        output: str = repr(result.stdout + result.stderr)
        match = re.search(GUID_PATTERN, output)

        if not match:
            raise ValueError("No active power scheme found.")

        active_power_scheme[match.group(2)] = match.group(1)
        return active_power_scheme
    except subprocess.CalledProcessError as e:
        raise e

def __get_available_power_schemes() -> dict[str, str]:
    """
    Get all the available power schemes on your system.
    Returns:
    dict[str, str]: A dictionary of available power scheme names and their GUIDs.
    """
    power_schemes: dict[str, str] = {}

    POWER_SCHEME_PATTERN: re.Pattern[str] = regex.get_guid_pattern()

    try:
        result: subprocess.CompletedProcess[str] = subprocess.run(
            ["powercfg", "/list"],
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            text=True,
            check=True,
            creationflags=subprocess.CREATE_NO_WINDOW
        )
        output: str = repr(result.stdout + result.stderr)
        available_power_schemes = re.finditer(POWER_SCHEME_PATTERN, output)

        if not available_power_schemes:
            raise ValueError("No power schemes found.")

        for match in available_power_schemes:
            power_schemes[match.group(2)] = match.group(1)
        return power_schemes
    except subprocess.CalledProcessError as e:
        raise e
    except Exception as e:
        raise e

def get_power_schemes() -> dict[str, str]:
    """
    Get all the available power schemes on your system.
    Returns:
        dict[str, str]: A dictionary of available power scheme names and their GUIDs.
    """
    try:
        return __get_available_power_schemes()
    except Exception as e:
        raise e

def change_power_scheme(power_scheme: str) -> int:
    """
    Changes the current power scheme to the specified one.
    Returns:
        int: The exit code of the process.
    """
    exit_code: int = 0

    ACTIVE_POWER_SCHEME: dict[str, str] = __get_active_power_scheme()
    AVAILABLE_POWER_SCHEMES: dict[str, str] = __get_available_power_schemes()

    if power_scheme not in AVAILABLE_POWER_SCHEMES:
        raise ValueError(f"Power scheme '{power_scheme}' is not available.")

    if power_scheme in ACTIVE_POWER_SCHEME:
        print(f"Power scheme '{power_scheme}' is already active.")
        return 2

    try:
        exit_code = process.run(['powercfg',
                                 '/setactive',
                                 AVAILABLE_POWER_SCHEMES[power_scheme]])
        return exit_code
    except subprocess.CalledProcessError as e:
        raise e
    except Exception as e:
        raise e
    finally:
        if exit_code == 0:
            print(f"Power scheme set to", power_scheme)
