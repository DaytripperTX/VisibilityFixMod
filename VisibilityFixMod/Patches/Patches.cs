using HarmonyLib;

namespace VisibilityFixMod.Patches
{
    class Patches
    {
        [HarmonyPatch(typeof(ScheduleOne.Stealth.PlayerVisibility))] 
        [HarmonyPatch("CalculateVisibility")]                  
        class VisibilityPatch
        {
            static bool Prefix(ref float __result)
            {
                __result = 10f; // Minimum visibility
                return false;  // Skip the original method
            }
        }
    }
}
