"""
This module check if the platform
is supported.
"""

if __name__ == "__main__":
    raise ImportError("This module cannot be used directly.")

import sys
from typing import LiteralString

def __get_current_platform() -> LiteralString:
    """
    Get the current platform.
    Returns:
        LiteralString: The current platform.
    """
    return sys.platform.lower()

def get() -> LiteralString:
    """
    Gets the current platform from an internal
    function and returns it.
    Returns:
        LiteralString: The current platform.
    """
    return __get_current_platform()
