using Gamelauncher.Core;
using System;
using System.Runtime.Versioning;

namespace ConsoleApp
{
    internal sealed class Program
    {
        [SupportedOSPlatform("windows")]
        private static void Run()
        {
            if (string.IsNullOrEmpty(Arguments.FileName))
            {
#if RELEASE
                Console.WriteLine("\n[ERROR] There is no program to run on!");
                Console.WriteLine("Press the ENTER key to close this program.");
                Console.ReadLine();
                Environment.Exit(1);
#elif DEBUG
                Console.WriteLine($"\n[DEBUG] Arguments.FileName is empty or null.");
                Environment.Exit(1);
#endif
            }

            Guid PREVIOUS_POWER_PLAN = PowerManagement.GetActivePowerPlan();
            Guid BALANCED_POWER_PLAN = PowerManagement.GetPowerPlan("Balanced");
            Guid HIGH_PERFORMANCE_PLAN = PowerManagement.GetPowerPlan("High performance");

            try
            {
                PowerManagement.SetPowerPlan(HIGH_PERFORMANCE_PLAN);
            }
            catch (Exception ex)
            {
                ErrorHandler.WaitAndExit(ex);
            }

            try
            {
                ProcessManager.CreateProcess(Arguments.FileName, null);
            }
            catch (Exception ex)
            {
                ErrorHandler.WaitAndExit(ex);
            }

            if (PREVIOUS_POWER_PLAN == BALANCED_POWER_PLAN)
            {
                PowerManagement.SetPowerPlan(PREVIOUS_POWER_PLAN);
            }
            else
            {
#if DEBUG
            PowerManagement.SetPowerPlan(BALANCED_POWER_PLAN);
            Console.WriteLine($"[DEBUG, Program.Run()]" +
                $"PREVIOUS_POWER_PLAN={PREVIOUS_POWER_PLAN}" +
                " was not {BALANCED_POWER_PLAN}" +
                " reverted back to Balanced.");
#endif
            }
        }

        [SupportedOSPlatform("windows")]
        private static int Main(string[] args)
        {
            try
            {
                Platform.IsPlatformCompatible();
            }
            catch (Exception ex)
            {
                ErrorHandler.WaitAndExit(ex);
            }

            try
            {
                Arguments.ParseArguments(args);
            }
            catch (Exception ex)
            {
                ErrorHandler.WaitAndExit(ex);
            }

            try
            {
                Run();
            }
            catch (Exception ex)
            {
                ErrorHandler.WaitAndExit(ex);
            }

            return 0;
        }
    }
}
