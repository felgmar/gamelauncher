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
            Console.WriteLine("Usage: {0} [/powerscheme <scheme>] [/run <file>]", ProgramInfo.GetName());
        }

        internal static void ParseArguments(string[] args)
        {
            for (int index = 0; index < args.Length; index++)
            {
                switch (args[index].Trim().ToLowerInvariant())
                {
                    case "/powerscheme":
                    case "/p":
                        PowerScheme = args[++index].ToLowerInvariant();
                        continue;
                    case "/run":
                    case "/r":
                        FileName = args[++index].ToLowerInvariant();
                        break;
                    case "/help":
                    case "/h":
                    case "/?":
                        ShowHelp();
                        break;
                    default:
                        ShowHelp();
                        throw new ArgumentException($"Invalid argument: {args[index++].ToLowerInvariant()}");
                }
            }

#if DEBUG
            Console.WriteLine($"""
            [DEBUG, Arguments.ParseArguments()] parameters.PowerScheme={PowerScheme}
            [DEBUG, Arguments.ParseArguments()] parameters.FileName={FileName}
            """);
#endif
        }
    }
}
