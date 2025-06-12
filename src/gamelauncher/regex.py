import re

"""
This module manages RegEx patterns
for different use cases.
"""

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
    return __get_guid_pattern()
g