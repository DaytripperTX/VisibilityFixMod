using MelonLoader;

[assembly: MelonInfo(typeof(VisibilityFixMod.VisibilityFix), "VisibilityFix", "1.0.4", "volcomtx")]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace VisibilityFixMod
{
    public class VisibilityFix : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Config.Load();
            var harmony = new HarmonyLib.Harmony("com.visibilityfix");
            harmony.PatchAll();
            LoggerInstance.Msg($"Visibility Fix loaded. MaxVisibility = {Config.MaxVisibility}");

            if (Config.EnableDebugLogs)
                LoggerInstance.Msg("Debug Logs Enabled");
        }

        public override void OnDeinitializeMelon()
        {
            ModManagerIntegration.Deinitialize();
        }

    }
}
