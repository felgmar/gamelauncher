using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    internal sealed class Program
    {
        private static void RunProcessAndRestorePowerScheme()
        {
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
                    Your platform {CURRENT_PLATFORM} is not supported. This program
                    only works on {PlatformID.Win32NT}.
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
                RunProcessAndRestorePowerScheme();
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
