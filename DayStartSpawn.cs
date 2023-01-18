using HarmonyLib;
using PotionCraft.ManagersSystem;
using PotionCraft.ManagersSystem.Npc;
using PotionCraft.Npc.Parts;
using System;
using System.Collections.Generic;

namespace UniqueCustomers
{
    class DayStartSpawn
    {
        [HarmonyPostfix, HarmonyPatch(typeof(NpcManager), "SpawnNpcOnDayStart")]
        public static void SpawnNpcOnDayStart_Postfix()
        {
            if(Plugin.customNpcTemplates == null || Plugin.customNpcTemplates.Count == 0)
            {
                // No customers
            }else
            {
                // Grab the current day
                int curDay = Managers.Day.currentDayAbsoluteNum;

                // Loop through the custom npc templates
                foreach (KeyValuePair<Tuple<NpcTemplate, string>, int> cn in Plugin.customNpcTemplates)
                {
                    // Check if we have customers for the current day
                    if(cn.Value == curDay)
                    {
                        // If the day matches, add them to the queue to spawn
                        Managers.Npc.AddToQueueForSpawn(cn.Key.Item1, null, null, false);
                        Managers.Npc.SpawnNpcQueue();
                        Plugin.Log.LogInfo($"NPC ({cn.Key.Item2}) added to today's queue");
                    }
                }
            }
        }
    }
}
