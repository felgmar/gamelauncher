using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    internal sealed partial class PowerManagement
    {
        private static readonly Dictionary<string, Guid> AVAILABLE_POWER_SCHEMES = new();

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
                        Console.WriteLine($"[DEBUG, PowerManagement.GetPowerSchemes()] Got a power scheme: name={name}, guid={guid}");
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

        private static bool IsPowerSchemeValid(Guid PowerPlanGuid)
        {
            string POWER_PLAN_NAME = string.Empty;
            bool IS_POWER_PLAN_GUID_VALID = Guid.TryParse(PowerPlanGuid.ToString(), out _);

            if (IS_POWER_PLAN_GUID_VALID) return true;

#if DEBUG
            string message = $"[DEBUG, PowerManagement.IsPowerSchemeValid()] IS_POWER_PLAN_GUID_VALID={IS_POWER_PLAN_GUID_VALID}," +
                " POWER_SCHEME_GUID=" + PowerPlanGuid;
            Console.WriteLine(message);
#else
            Console.WriteLine($"The power scheme {POWER_PLAN_NAME} is not valid.");
#endif

            return false;
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
        internal static Guid GetActivePowerPlan()
        {
            string ACTIVE_POWER_PLAN_NAME = string.Empty;
            Guid ACTIVE_POWER_PLAN_GUID = Guid.Empty;

            ProcessManager.CreateProcess("powercfg.exe", "/getactivescheme");

            Match match = Regex.Match(ProcessManager.ProcessOutput,
                                      RegexManager.GetGUIDPattern().ToString());

            if (!match.Success)
            {
                throw new InvalidDataException("Could not retrieve the current power plan GUID.");
            }

            try
            {
                GetAvailablePowerSchemes();
            }
            catch (Exception)
            {

                throw;
            }

            foreach (KeyValuePair<string, Guid> POWER_PLAN in AVAILABLE_POWER_SCHEMES)
            {
                if (POWER_PLAN.Key == match.Groups[2].Value)
                {
#if DEBUG
                    string ErrorMessage = $"[DEBUG, PowerManagement.GetActivePowerPlan()] Got active power plan: name={POWER_PLAN.Key}, guid={POWER_PLAN.Value}.";
                    Console.WriteLine(ErrorMessage);
#endif
                    ACTIVE_POWER_PLAN_NAME = POWER_PLAN.Key;
                    ACTIVE_POWER_PLAN_GUID = POWER_PLAN.Value;
                    break;
                }
            }

            return ACTIVE_POWER_PLAN_GUID;
        }

        internal static Guid GetPowerPlan(string PowerPlanName)
        {
            Guid POWER_PLAN_GUID = Guid.Empty;

            foreach (KeyValuePair<string, Guid> POWER_PLAN in AVAILABLE_POWER_SCHEMES)
            {
                if (PowerPlanName == POWER_PLAN.Key)
                {
#if DEBUG
                    Console.WriteLine($"[DEBUG, PowerManagement.GetPowerPlan()] Power plan received: name={POWER_PLAN.Key} guid={POWER_PLAN.Value}");
#endif
                    POWER_PLAN_GUID = POWER_PLAN.Value;
                    break;
                }
            }

            return POWER_PLAN_GUID;
        }

        internal static int SetPowerPlan(Guid PowerSchemeGuid)
        {
            string POWER_PLAN_NAME = string.Empty;

            foreach (KeyValuePair<string, Guid> POWER_SCHEME in AVAILABLE_POWER_SCHEMES)
            {
                if (POWER_SCHEME.Value == PowerSchemeGuid)
                {
                    POWER_PLAN_NAME = POWER_SCHEME.Key;
#if DEBUG
                    Console.WriteLine($"[DEBUG, PowerManagement.SetPowerPlan()] " + "Found a match for " + PowerSchemeGuid + ". POWER_SCHEME.Key=" +
                        POWER_SCHEME.Key);
#endif
                    break;
                }

#if DEBUG
                Console.WriteLine($"[DEBUG, PowerManagement.SetPowerPlan()] " + "POWER_SCHEME.Value=" +
                        POWER_SCHEME.Value + " no match for " + PowerSchemeGuid + ". Retrying...");
#else
                    Console.WriteLine($"The power scheme" +
                        POWER_SCHEME.Value + " does not match GUID " + PowerSchemeGuid + ". Retrying...");
#endif
            }

#if DEBUG
            string message = $"[DEBUG, PowerManagement.SetPowerPlan()] IS_POWER_SCHEME_GUID_VALID=" +
                IsPowerSchemeValid(PowerSchemeGuid) + " POWER_PLAN_NAME=" + POWER_PLAN_NAME;

            Console.WriteLine(message);
                    
#else
            Console.WriteLine($"The power plan {PowerSchemeGuid} is valid.");
#endif

            try
            {
                if (IsPowerSchemeValid(PowerSchemeGuid))
                {
                    ProcessManager.CreateProcess("powercfg.exe", $"/setactive {PowerSchemeGuid}");
                }
                else
                {
#if DEBUG
                    Console.WriteLine($"[DEBUG, PowerManagement.SetPowerPlan()] PowerSchemeGuid={PowerSchemeGuid}," +
                        " IsPowerSchemeValid=sPowerSchemeValid=" + IsPowerSchemeValid(PowerSchemeGuid));
#else
                    throw new InvalidDataException($"The power plan {POWER_PLAN_NAME} is not valid");
#endif
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
#if DEBUG
                Console.WriteLine($"[DEBUG, PowerManagement.SetPowerPlan()] Changed power plan. POWER_PLAN={POWER_PLAN_NAME}");
#else
                Console.WriteLine("Power plan was set to " + POWER_PLAN_NAME);
#endif
            }
            return ProcessManager.ExitCode;
        }
    }
}
