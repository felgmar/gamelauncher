using System;
using System.Text.RegularExpressions;

namespace Gamelauncher.Core
{
    /// <summary>
    /// Provides functionality for working with regular expressions related to GUIDs and their associated descriptions.
    /// </summary>
    /// <remarks>This class includes methods to retrieve a pre-defined regular expression for matching GUIDs
    /// followed by descriptions and to validate the format of a given GUID string. It is designed for internal use and
    /// is not intended for external consumption.</remarks>
    internal sealed partial class RegexManager
    {
        /// <summary>
        /// Creates a regular expression to match GUIDs followed by a description in parentheses.
        /// </summary>
        /// <remarks>The pattern matches a 36-character GUID (including hyphens) followed by one or more
        /// whitespace characters and a description enclosed in parentheses. The GUID is captured in the first
        /// group, and the description is captured in the second group.</remarks>
        /// <returns>A <see cref="Regex"/> instance configured with the specified pattern.</returns>
        [GeneratedRegex(@"([0-9a-fA-F\-]{36})\s+\((.*?)\)")]
        private static partial Regex GUIDPattern();

        internal static Regex GetGUIDPattern() => GUIDPattern();

        internal static bool IsGUIDValid(string guid)
        {
            bool IS_GUID_VALID = Guid.TryParse(guid, out _);

            return IS_GUID_VALID;
        }
    }

    /// <summary>
    /// Represents a globally unique identifier (GUID).
    /// </summary>
    /// <remarks>This class provides functionality related to GUIDs. It initializes the GUID pattern using the
    /// <see cref="RegexManager.GetGUIDPattern"/> method.</remarks>
    internal sealed class GUID
    {
        public GUID()
        {
            RegexManager.GetGUIDPattern();
        }
    }
}
