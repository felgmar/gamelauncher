"""
This module check if the platform
is supported.
"""

if __name__ == "__main__":
    raise ImportError("This module cannot be used directly.")

import sys
from typing import LiteralString

def __get_current_platform() -> LiteralString:
    return sys.platform.lower()

def get() -> LiteralString:
    return __get_current_platform()
