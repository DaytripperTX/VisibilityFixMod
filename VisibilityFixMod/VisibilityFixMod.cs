using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(VisibilityFixMod.VisibilityFix), "VisibilityFix", "0.1.0", "volcomtx")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace VisibilityFixMod
{
    public class VisibilityFix : MelonMod
    {
        public override void OnApplicationStart()
        {
            var harmony = new HarmonyLib.Harmony("com.visibilityfix");
            harmony.PatchAll();
            LoggerInstance.Msg("Visibility Fix Mod loaded and Harmony patched.");
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                MelonLogger.Msg("F1 key was pressed!");
            }
        }
    }
}
