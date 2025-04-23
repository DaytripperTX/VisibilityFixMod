using MelonLoader;
using System;
using ModManagerPhoneApp;

[assembly: MelonInfo(typeof(VisibilityFixMod.VisibilityFix), "VisibilityFix", "0.4.0", "volcomtx")]
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

            if(Config.EnableDebugLogs)
                LoggerInstance.Msg("Debug Logs Enabled");

            try
            {
                ModManagerPhoneApp.ModSettingsEvents.OnPreferencesSaved += Config.HandleSettingsUpdate;
                LoggerInstance.Msg("Successfully subscribed to Mod Manager save event.");
            }
            catch (Exception ex)
            {
                LoggerInstance.Warning($"Could not subscribe to Mod Manager event (Mod Manager may not be installed/compatible): {ex.Message}");
            }
        }

    }


    }
