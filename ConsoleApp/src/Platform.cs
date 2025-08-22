using System;

namespace ConsoleApp
{
    internal sealed class Platform
    {
        internal static PlatformID GetCurrentPlatform()
        {
            return Environment.OSVersion.Platform;
        }
    }
}
