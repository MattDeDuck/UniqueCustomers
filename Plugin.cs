using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json;
using PotionCraft.Npc.Parts;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System;

namespace UniqueCustomers
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, "1.1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log { get; set; }
        public static string pluginLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static Dictionary<Tuple<NpcTemplate, string>, int> customNpcTemplates = new();

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Log = this.Logger;
            Harmony.CreateAndPatchAll(typeof(Plugin));
            Harmony.CreateAndPatchAll(typeof(GameManagerStartPatch));
            Harmony.CreateAndPatchAll(typeof(DayStartSpawn));
            Harmony.CreateAndPatchAll(typeof(StartNightPatch));
        }

        public static void Setup ()
        {
            customNpcTemplates.Clear();            
        }

        public static void GenerateCustomers()
        {
            string json = File.ReadAllText(pluginLoc + "/customers.json");
            List<CustomCustomer> customCustomers = JsonConvert.DeserializeObject<List<CustomCustomer>>(json);

            foreach (var customer in customCustomers)
            {
                bool isExistingNPC = customNpcTemplates.Any(x => x.Key.Item2 == customer.NpcName);

                if(!isExistingNPC)
                {
                    Customer.CreateCustomer(customer.NpcToCopy, customer.NpcName, customer.QuestName, customer.QuestText, customer.QuestDesiredEffects, customer.DayToAppear, customer.HasCustomText, customer.CustMandatory, customer.CustOptional, customer.DiscussionOptionText, customer.SecondText);
                }else
                {
                    //Log.LogWarning($"'{customer.npcName}' NPC already exists, so it doesn't need to be added again");
                }
            }

            //debug
            foreach(KeyValuePair<Tuple<NpcTemplate, string>, int> ct in customNpcTemplates)
            {
                Log.LogInfo($"NPC name: {ct.Key.Item2} -- Day to appear {ct.Value}");
            }
        }

        public class CustomCustomer
        {
            public string NpcToCopy { get; set; }
            public string NpcName { get; set; }
            public string QuestName { get; set; }
            public string QuestText { get; set; }
            public List<string> QuestDesiredEffects { get; set; }
            public int DayToAppear { get; set; }
            public Dictionary<string, string> CustMandatory { get; set; }
            public Dictionary<string, string> CustOptional { get; set; }
            public bool HasCustomText { get; set; }
            public string DiscussionOptionText { get; set; }
            public string SecondText { get; set; }
        }
    }
}