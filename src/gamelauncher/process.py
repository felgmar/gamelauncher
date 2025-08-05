"""
This module handles processes.
"""

if __name__ == "__main__":
    raise ImportError("This module cannot be used directly.")

import subprocess

def __run_process(command: list[str]) -> int:
    """
    Run a command in a subprocess and return the result.
    Args:
        command (list[str]): The command to run.
    Returns:
        subprocess.CompletedProcess[str]: The result of the command.
    """
    exit_code: int = 0
    try:
        process = subprocess.run(
            command,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            check=True,
            creationflags=subprocess.CREATE_NO_WINDOW
        )
        exit_code: int = process.returncode
        return exit_code
    except subprocess.CalledProcessError as e:
        raise e
    except Exception as e:
        raise e

def run(command: list[str]) -> int:
    return __run_process(command)
