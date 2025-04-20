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

        //Debug method: Tells us the total visibility score of a player
        [HarmonyPatch(typeof(ScheduleOne.Stealth.PlayerVisibility), "CalculateVisibility")]
        class DebugVisibilityPatch
        {
            static private float lastLogTime = 0f;
            static private readonly float logCooldown = 0.5f; // seconds

            static void Postfix(float __result)
            {
                if (Time.time - lastLogTime >= logCooldown)
                {
                    lastLogTime = Time.time;
                    Msg($"[Debug] Final Visibility Score (after clamp): {__result}");
                }
            }
        }

        //Debug method: Lets us see different visibility attributes and their values
        [HarmonyPatch(typeof(ScheduleOne.Stealth.PlayerVisibility), "CalculateVisibility")]
        class DebugVisibilityAttributes
        {
            static private float lastLogTime = 0f;
            private static readonly float logCooldown = 0.5f; // seconds

            static void Prefix(ScheduleOne.Stealth.PlayerVisibility __instance)
            {

                if (Time.time - lastLogTime >= logCooldown)
                {
                    lastLogTime = Time.time;

                    if (__instance.activeAttributes == null)
                    {
                        return;
                    }

                    foreach (var attr in __instance.activeAttributes)
                    {
                        Msg($"[Debug] Attr: +{attr.pointsChange}, Mult: {attr.multiplier}, Type: {attr.name}");
                    }
                }
            }
        }
    }
}



