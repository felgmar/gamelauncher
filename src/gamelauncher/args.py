"""
This modules handles arguments parsing.
"""

if __name__ == "__main__":
    raise ImportError("This module cannot be used directly.")

import argparse

from gamelauncher import power_management

def sanitize_arguments(path: str) -> str:
    """
    Handles arguments and sanitizes them.
    Returns:
        str: The target path.
    """
    if not path.lower().endswith(".lnk"):
        return path
    try:
        import win32com.client
        shell = win32com.client.Dispatch("WScript.Shell")
        shortcut = shell.CreateShortcut(path)
        return shortcut.TargetPath
    except Exception as e:
        raise e

def init() -> tuple[argparse.Namespace, list[str]]:
    argparser = argparse.ArgumentParser()

    try:
        available_power_schemes: dict[str, str] = power_management.__get_available_power_schemes()
        argparser.add_argument("-p",
                            "--power-scheme",
                            action="store",
                            help="Specify the power scheme to use. Available modes are: " +
                                ', '.join(available_power_schemes),
                            default="Balanced",
                            choices=available_power_schemes)
        args, unknown_args = argparser.parse_known_args()
        return args, unknown_args
    except Exception as e:
        raise e
