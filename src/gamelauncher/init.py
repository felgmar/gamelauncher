"""
The main module for the gamelauncher.
Where it gets initialized and run.
"""

from gamelauncher import platform
from gamelauncher import args as arguments
from gamelauncher import power_management
from gamelauncher import process

def main():
    """
    Initializes and runs the main entry point of the application.
    """
    try:
        args, unknown_args = arguments.init()
        if not unknown_args:
            raise ValueError("No command to run.")

        available_power_schemes: dict[str, str] = power_management.get_power_schemes()

        if not args.power_scheme:
            if "Ultimate performance" in available_power_schemes:
                power_management.change_power_scheme("Ultimate performance")
            elif "High performance" in available_power_schemes:
                power_management.change_power_scheme("High performance")
            else:
                print("[!] No high performance power scheme was found. The Balanced power scheme will be used.")
                power_management.change_power_scheme("Balanced")
        else:
            power_management.change_power_scheme(args.power_scheme)

        process.run(unknown_args)

        power_management.change_power_scheme("Balanced")
    except Exception as e:
        raise e

if __name__ == "__main__":
    CURRENT_PLATFORM: str = platform.get()

    if not platform.get() == "linux":
        raise RuntimeError("Your platform is not supported:", CURRENT_PLATFORM)

    try:
        main()
    except Exception as e:
        raise e
