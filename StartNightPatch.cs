using HarmonyLib;
using PotionCraft.ManagersSystem.Environment;

namespace UniqueCustomers
{
    class StartNightPatch
    {
        [HarmonyPostfix, HarmonyPatch(typeof(EnvironmentManager), "StartNight")]
        public static void StartNight_Postfix()
        {
            Plugin.GenerateCustomers();
            Plugin.Log.LogInfo($"Updated customers. ({Plugin.customNpcTemplates.Count}) in total");
        }
    }
}
