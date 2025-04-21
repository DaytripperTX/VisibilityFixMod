using HarmonyLib;
using UnityEngine;
using static MelonLoader.MelonLogger;
using System.Linq;
using ScheduleOne.Stealth;
using ScheduleOne.Vision;


namespace VisibilityFixMod.Patches
{
    class Patches
    {
        [HarmonyPatch(typeof(PlayerVisibility))]
        [HarmonyPatch("CalculateVisibility")]
        class VisibilityPatch
        {
            static bool Prefix(PlayerVisibility __instance, ref float __result)
            {
                var attrs = __instance.activeAttributes;
                if (attrs == null)
                {
                    __result = 0f;
                    return false;
                }

                if (Config.EnableDebugLogs)
                {
                    Msg("[DEBUG] Raw activeAttributes:");
                    foreach (var attr in attrs)
                    {
                        Msg($"[DEBUG]   {attr.name} | +{attr.pointsChange}, x{attr.multiplier}");
                    }
                }

                // Step 1: Filter unique-max attributes by uniquenessCode
                var uniqueMax = attrs
                    .OfType<UniqueVisibilityAttribute>()
                    .GroupBy(a => a.uniquenessCode)
                    .Select(group => group.OrderByDescending(a => a.pointsChange).First())
                    .ToDictionary(attr => attr.uniquenessCode, attr => attr.pointsChange);

                // Step 2: Rebuild filtered attribute list
                var filtered = attrs
                    .Where(attr =>
                    {
                        if (attr is UniqueVisibilityAttribute uniqueAttr)
                        {
                            return uniqueMax.TryGetValue(uniqueAttr.uniquenessCode, out float max) &&
                                   uniqueAttr.pointsChange >= max;
                        }
                        return true;
                    })
                    .ToList();

                if (Config.EnableDebugLogs)
                {
                    Msg("[DEBUG] Filtered activeAttributes (after uniqueness logic):");
                    foreach (var attr in filtered)
                    {
                        Msg($"[DEBUG]   {attr.name} | +{attr.pointsChange}, x{attr.multiplier}");
                    }
                }

                // Step 3: Apply contribution logic
                float visibility = 0f;

                foreach (var attr in filtered)
                {
                    float points = attr.pointsChange;
                    float multiplier = attr.multiplier;
                    string name = attr.name.ToLowerInvariant();

                    //base visibility
                    if (name.Contains("base visibility"))
                        points = Config.BaseVisibility;

                    // Override multiplier with config-defined values
                    if (name.Contains("sneaky"))
                        multiplier = Config.ActiveMultipliers.Sneaky;

                    if (name.Contains("crouched"))
                        multiplier = Config.ActiveMultipliers.Crouched;

                    if (name.Contains("flashlight"))
                    {
                        multiplier = Config.FlashlightAffectsSneak
                            ? Config.ActiveMultipliers.Flashlight
                            : 1f;

                        if (!Config.FlashlightAffectsSneak)
                            points = 0f;
                    }


                    visibility += points;

                    if (multiplier != 1f)
                        visibility *= multiplier;

                    if (Config.EnableDebugLogs)
                    {
                        Msg($"[DEBUG] Attr: {attr.name} | +{points}, x{multiplier}, Running Total: {visibility}");
                    }
                }

                __result = Mathf.Clamp(visibility, 0f, Config.MaxVisibility);

                if (Config.EnableDebugLogs)
                {
                    Msg($"[DEBUG] Final calculated visibility: {__result}");
                }

                return false;
            }
        }
        
        [HarmonyPatch(typeof(PlayerVisibility), "CalculateVisibility")]
        public static class DebugVisibility
        {
            //Debug method: Tells us the total visibility score of a player
            [HarmonyPrefix]
            static void Prefix(ScheduleOne.Stealth.PlayerVisibility __instance)
            {
                if (!Config.EnableDebugLogs || !LogRateLimiter.CanLog("PreVisibility", 0.5f))
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
                if (Config.EnableDebugLogs && LogRateLimiter.CanLog("PostVisibility", 0.5f))
                {
                    Msg($"[Debug] Final Visibility Score (after clamp): {__result}");
                }
            }
        }
    }
}



