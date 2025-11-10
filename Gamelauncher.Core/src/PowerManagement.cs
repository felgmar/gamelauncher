using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Gamelauncher.Core
{
    public sealed partial class PowerManagement
    {
        private static readonly Dictionary<string, Guid> AVAILABLE_POWER_SCHEMES = [];

        private static void GetPowerSchemes()
        {
            MatchCollection? Matches = null;

            try
            {
                ProcessManager.CreateProcess("powercfg.exe", "/list");
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                Matches ??= RegexManager.GetGUIDPattern().Matches(ProcessManager.ProcessOutput);
            }
            catch (Exception)
            {
                throw;
            }

            foreach (Match match in Matches)
            {
                if (match.Success && match.Groups.Count > 2)
                {
                    string name = match.Groups[2].Value;
                    Guid guid = Guid.Parse(match.Groups[1].Value.Trim());
#if DEBUG
                    Console.WriteLine($"[DEBUG, PowerManagement.GetPowerSchemes()] Got power scheme: name={name}, guid={guid}");
#endif
                    AVAILABLE_POWER_SCHEMES.Add(name, guid);
                }
            }
        }

        public PowerManagement()
        {
            GetPowerSchemes();
        }

        private static bool IsPowerSchemeValid(Guid PowerPlanGuid)
        {
            string POWER_PLAN_NAME = string.Empty;
            bool IS_POWER_PLAN_GUID_VALID = RegexManager.IsGUIDValid(PowerPlanGuid.ToString());

            if (IS_POWER_PLAN_GUID_VALID) return true;

#if DEBUG
            string ErrorMessage = "[DEBUG, PowerManagement.IsPowerSchemeValid()]" +
                $" PowerPlanGuid={PowerPlanGuid}, " +
                $"IS_POWER_PLAN_GUID_VALID={IS_POWER_PLAN_GUID_VALID}";
            Console.WriteLine(ErrorMessage);
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
                    throw new InvalidDataException("No power schemes could be retrieved...");
                }
            }
            catch (Exception)
            {
                throw;
            }

            return AVAILABLE_POWER_SCHEMES;
        }
        public static Guid GetActivePowerPlan()
        {
            string ACTIVE_POWER_PLAN_NAME = string.Empty;
            Guid ACTIVE_POWER_PLAN_GUID = Guid.Empty;

            ProcessManager.CreateProcess("powercfg.exe", "/getactivescheme");

            Match Match = Regex.Match(ProcessManager.ProcessOutput,
                                      RegexManager.GetGUIDPattern().ToString());

            if (!Match.Success)
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
                if (POWER_PLAN.Key == Match.Groups[2].Value)
                {
#if DEBUG
                    string ErrorMessage = "[DEBUG, PowerManagement.GetActivePowerPlan()]" +
                        $"Got active power plan: name={POWER_PLAN.Key}, guid={POWER_PLAN.Value}.";
                    Console.WriteLine(ErrorMessage);
#endif
                    ACTIVE_POWER_PLAN_NAME = POWER_PLAN.Key;
                    ACTIVE_POWER_PLAN_GUID = POWER_PLAN.Value;
                    break;
                }
            }

            return ACTIVE_POWER_PLAN_GUID;
        }

        public static Guid GetPowerPlan(string PowerPlanName)
        {
            Guid POWER_PLAN_GUID = Guid.Empty;

            foreach (KeyValuePair<string, Guid> POWER_PLAN in AVAILABLE_POWER_SCHEMES)
            {
                if (PowerPlanName == POWER_PLAN.Key)
                {
#if DEBUG
                    string DebugMessage = $"[DEBUG, PowerManagement.GetPowerPlan()] Power plan received: " +
                        $"name={POWER_PLAN.Key}, guid={POWER_PLAN.Value}";
                    Console.WriteLine(DebugMessage);
#endif
                    POWER_PLAN_GUID = POWER_PLAN.Value;
                    break;
                }
            }

            return POWER_PLAN_GUID;
        }

        public static int SetPowerPlan(Guid PowerSchemeGuid)
        {
            string POWER_PLAN_NAME = string.Empty;

            foreach (KeyValuePair<string, Guid> POWER_SCHEME in AVAILABLE_POWER_SCHEMES)
            {
                if (POWER_SCHEME.Value == PowerSchemeGuid)
                {
                    POWER_PLAN_NAME = POWER_SCHEME.Key;
#if DEBUG
                    Console.WriteLine($"[DEBUG, PowerManagement.SetPowerPlan()] " +
                        $"Found a match for GUID={PowerSchemeGuid}, POWER_SCHEME.Key={POWER_SCHEME.Key}");
#endif
                    break;
                }
#if DEBUG
                Console.WriteLine($"[DEBUG, PowerManagement.SetPowerPlan()] " + "POWER_SCHEME.Value=" +
                    POWER_SCHEME.Value + " no match for " + PowerSchemeGuid + ". Retrying...");
#else
                string InfoMessage = $"The power scheme {POWER_SCHEME.Key} does not match GUID {PowerSchemeGuid}. Retrying...";
                Console.WriteLine(InfoMessage);
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
                if (!IsPowerSchemeValid(PowerSchemeGuid))
                {
#if DEBUG
                    throw new InvalidDataException($"[DEBUG, PowerManagement.SetPowerPlan()] " +
                        $"PowerSchemeGuid={PowerSchemeGuid}, IsPowerSchemeValid={IsPowerSchemeValid(PowerSchemeGuid)}");
#else
                    throw new InvalidDataException($"The power plan {POWER_PLAN_NAME} is not valid");
#endif
                }

                ProcessManager.CreateProcess("powercfg.exe", $"/setactive {PowerSchemeGuid}");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
#if DEBUG
                Console.WriteLine($"[DEBUG, PowerManagement.SetPowerPlan()] The power plan was changed to {POWER_PLAN_NAME}");
#else
                Console.WriteLine("Power plan was set to " + POWER_PLAN_NAME);
#endif
            }
            return ProcessManager.ExitCode;
        }
    }
}
