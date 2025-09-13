using System;
using System.Runtime.Versioning;

namespace ConsoleApp
{
    internal sealed class Program
    {
        [SupportedOSPlatform("windows")]
        private static void Run()
        {
#if RELEASE
            if (string.IsNullOrEmpty(Arguments.FileName))
            {
                Console.WriteLine("\n[ERROR] There is no program to run on!");
                Console.WriteLine("Press the ENTER key to close this program.");
                Console.ReadLine();
                Environment.Exit(1);
#endif

            Guid PREVIOUS_POWER_PLAN = PowerManagement.GetActivePowerPlan();
            Guid BALANCED_POWER_PLAN = PowerManagement.GetPowerPlan("Balanced");
            Guid HIGH_PERFORMANCE_PLAN = PowerManagement.GetPowerPlan("High performance");

            try
            {
                PowerManagement.SetPowerPlan(HIGH_PERFORMANCE_PLAN);
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                ProcessManager.CreateProcess(Arguments.FileName, null);
            }
            catch (Exception)
            {
                throw;
            }

            if (PREVIOUS_POWER_PLAN == BALANCED_POWER_PLAN)
            {
                PowerManagement.SetPowerPlan(PREVIOUS_POWER_PLAN);
            }
            else
            {
#if DEBUG
                Console.WriteLine($"[DEBUG, Program.Run()] PREVIOUS_POWER_PLAN=" +
                    PREVIOUS_POWER_PLAN + " was not " + BALANCED_POWER_PLAN +
                    " reverted back to Balanced.");
#endif
                PowerManagement.SetPowerPlan(BALANCED_POWER_PLAN);
            }
        }

        [SupportedOSPlatform("windows")]
        private static int Main(string[] args)
        {
            try
            {
                Platform.IsPlatformValid();
            }
            catch (Exception)
            {
                throw;
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

            return 0;
        }
    }
}
