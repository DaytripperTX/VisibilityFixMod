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
            Config.Load();
            var harmony = new HarmonyLib.Harmony("com.visibilityfix");
            harmony.PatchAll();
            LoggerInstance.Msg($"Visibility Fix loaded. MaxVisibility = {Config.MaxVisibility}");
        }

        public override void OnUpdate()
        {
            //Used for debugging purposes
            if (Input.GetKeyDown(KeyCode.F1)) 
            {
                MelonLogger.Msg("F1 key was pressed!");
            }
        }
    }
}
