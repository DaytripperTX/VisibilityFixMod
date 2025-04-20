using HarmonyLib;
using UnityEngine;
using static MelonLoader.MelonLogger;


namespace VisibilityFixMod.Patches
{
    class Patches
    {/*
        [HarmonyPatch(typeof(ScheduleOne.Stealth.PlayerVisibility))] 
        [HarmonyPatch("CalculateVisibility")]                  
        class VisibilityPatch
        {
            static bool Prefix(ref float __result)
            {
                __result = Config.MaxVisibility; // Minimum visibility
                return false;  // Skip the original method
            }
        }*/

        [HarmonyPatch(typeof(ScheduleOne.Stealth.PlayerVisibility), "CalculateVisibility")]
        public static class DebugVisibility
        {
            //Debug method: Tells us the total visibility score of a player
            [HarmonyPrefix]
            static void Prefix(ScheduleOne.Stealth.PlayerVisibility __instance)
            {
                if (!LogRateLimiter.CanLog("PreVisibility", 0.5f))
                    return;

                if (__instance.activeAttributes == null)
                    return;

                foreach (var attr in __instance.activeAttributes)
                {
                    Msg($"[Debug] Attr: +{attr.pointsChange}, Mult: {attr.multiplier}, Type: {attr.name}");
                }
            }

            //Debug method: Lets us see different visibility attributes and their values
            [HarmonyPostfix]
            static void Postfix(float __result)
            {
                if (LogRateLimiter.CanLog("PostVisibility", 0.5f))
                {
                    Msg($"[Debug] Final Visibility Score (after clamp): {__result}");
                }
            }
        }
    }
}



