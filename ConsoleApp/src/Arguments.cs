using System;
using System.Runtime.Versioning;

namespace ConsoleApp
{
    [SupportedOSPlatform("windows")]
    internal sealed class Arguments
    {
        internal static string PowerScheme { get; private set; } = string.Empty;
        internal static string FileName { get; private set; } = string.Empty;

        private static void ShowHelp()
        {
            Console.WriteLine("Usage: ConsoleApp [/powerscheme <scheme>] [/run <file>]");
        }

        internal static void ParseArguments(string[] args)
        {
            foreach (string arg in args)
            {
                switch (arg.ToLowerInvariant())
                {
                    case "/powerscheme":
                    case "/p":
                        PowerScheme = args[Array.IndexOf(args, arg) + 1].ToLowerInvariant();
                        continue;
                    case "/run":
                    case "/r":
                        FileName = args[Array.IndexOf(args, arg) + 1].ToLowerInvariant();
                        break;
                    case "/help":
                    case "/h":
                    case "/?":
                        ShowHelp();
                        break;
                    default:
                        break;
                }
            }

#if DEBUG
            Console.WriteLine($"""
            [DEBUG, ParseArguments()] parameters.PowerScheme={PowerScheme}
            [DEBUG, ParseArguments()] parameters.FileName={FileName}
            """);
            Console.WriteLine();
#endif
        }
    }
}
