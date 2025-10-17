using System;
using System.Text.RegularExpressions;

namespace Gamelauncher.Core
{
    internal sealed class GUID
    {
        public GUID()
        {
            RegexManager.GetGUIDPattern();
        }
    }

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
}
