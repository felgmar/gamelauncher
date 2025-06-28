"""
This module manages RegEx patterns
for different use cases.
"""

import re

if __name__ == "__main__":
    raise ImportError("This module cannot be used directly.")

def __get_guid_pattern() -> re.Pattern[str]:
    """
    Get the regex pattern for matching GUIDs.
    Returns:
        str: The regex pattern for GUIDs.
    """
    return re.compile(
        r"([0-9a-fA-F\-]{36})\s+\((.*?)\)",
        re.IGNORECASE
    )

def get_guid_pattern() -> re.Pattern[str]:
    """
    Get the regex pattern for matching GUIDs.
    Returns:
        re.Pattern[str]: The regex pattern for GUIDs.
    """
    return __get_guid_pattern()
