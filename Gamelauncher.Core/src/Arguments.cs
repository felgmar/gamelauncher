using System;
using System.Runtime.Versioning;

namespace Gamelauncher.Core
{
    [SupportedOSPlatform("windows")]
    public sealed class Arguments
    {
        public static string PowerScheme { get; private set; } = string.Empty;
        public static string FileName { get; private set; } = string.Empty;

        private static void ShowHelp()
        {
            Console.WriteLine("Usage: {0} [/powerscheme <scheme>] [/run <file>]", ProgramInfo.GetName());
        }

        public static void ParseArguments(string[] args)
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
                        Console.WriteLine($"Unknown argument: {args[++index].ToLowerInvariant()}");
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
