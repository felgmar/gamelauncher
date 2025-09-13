using System;

namespace ConsoleApp
{
    internal sealed class Platform(bool IsPlatformSupported)
    {
        private readonly bool IS_PLATFORM_SUPPORTED = IsPlatformSupported;

        private static PlatformID GetCurrentPlatform() => Environment.OSVersion.Platform;

        internal static void IsPlatformValid()
        {
            Platform platform = new(GetCurrentPlatform() == PlatformID.Win32NT);

#if DEBUG
            Console.WriteLine($"[DEBUG, Platform.IsPlatformValid()] IS_PLATFORM_SUPPORTED={platform.IS_PLATFORM_SUPPORTED}");
#endif

            if (!platform.IS_PLATFORM_SUPPORTED)
            {
                string ErrorMessage = $"""
                    Your platform ({GetCurrentPlatform()}) is not supported.
                    This program only works on {PlatformID.Win32NT}.
                 """;
                throw new PlatformNotSupportedException(ErrorMessage);
            }
        }
    }
}
