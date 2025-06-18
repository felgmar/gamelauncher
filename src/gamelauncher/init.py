from gamelauncher import platform
from gamelauncher import args as arguments
from gamelauncher import power_management
from gamelauncher import process

def main():
    try:
        args, unknown_args = arguments.init()
        if not unknown_args:
            raise ValueError("No command to run.")

        if not args.power_scheme or not args.power_scheme == "Balanced":
            power_management.change_power_scheme(args.power_scheme)
        else:
            power_management.change_power_scheme("High performance")

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
