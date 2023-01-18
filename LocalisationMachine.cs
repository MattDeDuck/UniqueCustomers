using PotionCraft.LocalizationSystem;
using System.Collections.Generic;

namespace UniqueCustomers
{
    class LocalisationMachine
    {
        //static readonly Dictionary<string, string> localDict = LocalizationManager.;

        public static bool Exists(string key)
        {
            return LocalizationManager.ContainsKey(key);
        }

        public static void CreateEntry(string key, string value)
        {
            if(Exists(key))
            {
                return;
            }

            LocalizationManager.localizationData.Add(0, key, value);
        }
    }
}
