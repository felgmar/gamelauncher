using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    internal sealed partial class PowerManagement
    {
        private static Dictionary<string, Guid> AVAILABLE_POWER_SCHEMES = new();

        private static void GetPowerSchemes()
        {
            try
            {
                ProcessManager.CreateProcess("powercfg.exe", "/list");

                MatchCollection matches =
                    RegexManager.GetGUIDPattern().Matches(ProcessManager.ProcessOutput);

                foreach (Match match in matches)
                {
                    if (match.Success && match.Groups.Count > 2)
                    {
                        string name = match.Groups[2].Value;
                        Guid guid = Guid.Parse(match.Groups[1].Value.Trim());
#if DEBUG
                        Console.WriteLine($"[DEBUG, GetPowerSchemes()]: name={name}, guid={guid}");
#endif
                        AVAILABLE_POWER_SCHEMES.Add(name, guid);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static Dictionary<string, Guid> GetAvailablePowerSchemes()
        {
            try
            {
                if (AVAILABLE_POWER_SCHEMES.Count == 0)
                {
                    GetPowerSchemes();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return AVAILABLE_POWER_SCHEMES;
        }

        internal static bool IsPowerSchemeValid(string PowerSchemeName, Guid PowerSchemeGuid)
        {
            bool IS_POWER_SCHEME_GUID_VALID = Guid.TryParse(PowerSchemeGuid.ToString(), out _);

            if (!IS_POWER_SCHEME_GUID_VALID)
            {
                Console.WriteLine($"The power scheme {PowerSchemeName} is not valid.");
                return false;
            }

            return true;
        }

        internal static Dictionary<string, Guid> GetCurrentPowerPlan()
        {
            Dictionary<string, Guid> CURRENT_POWER_PLAN = new();

            ProcessManager.CreateProcess("powercfg.exe", "/getactivescheme");

            Match match = Regex.Match(ProcessManager.ProcessOutput,
                                      RegexManager.GetGUIDPattern().ToString());

            GetAvailablePowerSchemes();

            if (match.Success)
            {
                foreach (KeyValuePair<string, Guid> POWER_PLAN in AVAILABLE_POWER_SCHEMES)
                {
                    if (POWER_PLAN.Key == match.Groups[2].Value)
                    {
#if DEBUG
                        string ErrorMessage = $"[DEBUG, GetCurrentPowerPlan()] name={POWER_PLAN.Key}, guid={POWER_PLAN.Value}.";
                        Console.WriteLine(ErrorMessage);
#endif
                        if (CURRENT_POWER_PLAN.Count != 0) CURRENT_POWER_PLAN.Clear();
                        CURRENT_POWER_PLAN.Add(POWER_PLAN.Key, POWER_PLAN.Value);
                        break;
                    }
                }
            }
            else
            {
                throw new Exception("Could not retrieve the current power plan GUID.");
            }

            return CURRENT_POWER_PLAN;
        }

        internal static Dictionary<string, Guid> GetHighPerformancePlan()
        {
            Dictionary<string, Guid> HIGH_PERFORMANCE_POWER_PLAN = new();
            Dictionary<string, Guid> AVAILABLE_POWER_PLANS = GetAvailablePowerSchemes();

            foreach (KeyValuePair<string, Guid> POWER_PLAN in AVAILABLE_POWER_PLANS)
            {
                if (POWER_PLAN.Key == "High performance" ||
                    POWER_PLAN.Key == "Ultimate performance")
                {
#if DEBUG
                    Console.WriteLine($"[DEBUG, GetHighPerformancePlan()] Power scheme GUID is {POWER_PLAN.Value}");
#endif
                    HIGH_PERFORMANCE_POWER_PLAN.Add(POWER_PLAN.Key, POWER_PLAN.Value);
                    break;
                }
            }

            return HIGH_PERFORMANCE_POWER_PLAN;
        }

        internal static int SetPowerPlan(string PowerSchemeName, Guid PowerSchemeGuid)
        {
            try
            {
                bool IS_POWER_SCHEME_GUID_VALID = Guid.TryParse(PowerSchemeGuid.ToString(), out _);

                if (!IS_POWER_SCHEME_GUID_VALID)
                {
                    throw new InvalidDataException($"The GUID {PowerSchemeGuid} is not valid.");
                }
#if DEBUG
                Console.WriteLine($"[DEBUG, SetPowerPlan()] Power scheme set to {PowerSchemeName}");
#endif
                ProcessManager.CreateProcess("powercfg.exe", $"/setactive {PowerSchemeGuid}");
            }
            catch (Exception)
            {
                throw;
            }

            return ProcessManager.ExitCode;
        }
    }
}
