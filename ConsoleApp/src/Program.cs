using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    internal sealed class Program
    {
        private static void Run()
        {
            if (string.IsNullOrEmpty(Arguments.FileName))
            {
                Console.WriteLine("\n[ERROR] There is no program to run on!");
                Console.WriteLine("Press the ENTER key to close this program.");
                Console.ReadLine();
                Environment.Exit(1);
            }

            Dictionary<string, Guid> CURRENT_POWER_PLAN = PowerManagement.GetCurrentPowerPlan();
            Dictionary<string, Guid> HIGH_PERFORMANCE_PLAN = PowerManagement.GetHighPerformancePlan();

            foreach (KeyValuePair<string, Guid> POWER_PLAN in HIGH_PERFORMANCE_PLAN)
            {
                PowerManagement.SetPowerPlan(POWER_PLAN.Key, POWER_PLAN.Value);
                break;
            }

            try
            {
                ProcessManager.CreateProcess(Arguments.FileName, null);
            }
            catch (Exception)
            {
                throw;
            }

            foreach (KeyValuePair<string, Guid> POWER_PLAN in CURRENT_POWER_PLAN)
            {
                PowerManagement.SetPowerPlan(POWER_PLAN.Key, POWER_PLAN.Value);
                break;
            }
        }

        private static int Main(string[] args)
        {
            PlatformID CURRENT_PLATFORM = Platform.GetCurrentPlatform();

            if (CURRENT_PLATFORM != PlatformID.Win32NT)
            {
                string ErrorMessage = $"""
                    Your platform ({CURRENT_PLATFORM}) is not supported. This program was made for {PlatformID.Win32NT} only.
                    """;
                throw new PlatformNotSupportedException(ErrorMessage);
            }

            try
            {
                Arguments.ParseArguments(args);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                Run();
            }
            catch (Exception)
            {
                throw;
            }
#if DEBUG
            Console.WriteLine($"[DEBUG, Main()] Current platform: {CURRENT_PLATFORM}");
#endif
            return 0;
        }
    }
}
