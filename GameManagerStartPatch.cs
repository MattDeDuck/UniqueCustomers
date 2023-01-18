using HarmonyLib;
using PotionCraft.ManagersSystem.Game;
using PotionCraft.SceneLoader;

namespace UniqueCustomers
{
    class GameManagerStartPatch
    {
        [HarmonyPostfix, HarmonyPatch(typeof(GameManager), "Start")]
        public static void Start_Postfix()
        {
            ObjectsLoader.AddLast("SaveLoadManager.SaveNewGameState", () => Plugin.Setup());
        }
    }
}
