using System;

namespace Gamelauncher.Core
{
    public sealed class Platform()
    {
        private static PlatformID GetCurrentPlatform() => Environment.OSVersion.Platform;
        readonly static bool IS_PLATFORM_COMPATIBLE = GetCurrentPlatform() == PlatformID.Win32NT;

        public static void IsPlatformCompatible()
        {
#if DEBUG
            Console.WriteLine("[DEBUG, Platform.IsPlatformCompatible()] " +
                $"CURRENT_PLATFORM={GetCurrentPlatform()}, IS_PLATFORM_COMPATIBLE={IS_PLATFORM_COMPATIBLE}");
#endif
            if (!IS_PLATFORM_COMPATIBLE)
            {
                string ErrorMessage = $"""
                    Your platform ({GetCurrentPlatform()}) is not supported.
                    This program was designed for {PlatformID.Win32NT} only.
                 """;
                throw new PlatformNotSupportedException(ErrorMessage);
            }
        }
    }
}
