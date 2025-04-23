using MelonLoader;
using ModManagerPhoneApp;
using static MelonLoader.MelonLogger;

namespace VisibilityFixMod
{
    public static class ModManagerIntegration
    {
        private static MelonPreferences_Category category;

        public static void Initialize()
        {
            try
            {
                category = MelonPreferences.CreateCategory("VisibilityFix", "Main Settings");
                category.CreateEntry("EnableDebugLogs", Config.EnableDebugLogs);
                category.CreateEntry("FlashlightAffectsSneak", Config.FlashlightAffectsSneak);
                category.CreateEntry("BaseVisibility", Config.BaseVisibility);
                category.CreateEntry("MaxVisibility", Config.MaxVisibility);

                ModSettingsEvents.OnPreferencesSaved += ApplySettings;
                Msg("ModManager integration initialized.");
            }
            catch (System.Exception ex)
            {
                Warning($"[ModManagerIntegration] Could not initialize: {ex.Message}");
            }
        }

        public static void ApplySettings()
        {
            try
            {
                Config.EnableDebugLogs = category.GetEntry<bool>("EnableDebugLogs").Value;
                Config.FlashlightAffectsSneak = category.GetEntry<bool>("FlashlightAffectsSneak").Value;
                Config.BaseVisibility = category.GetEntry<float>("BaseVisibility").Value;
                Config.MaxVisibility = category.GetEntry<float>("MaxVisibility").Value;

                Config.Save();
                Msg("[ModManagerIntegration] Settings applied and saved.");
            }
            catch (System.Exception ex)
            {
                Warning($"[ModManagerIntegration] Failed to apply settings: {ex.Message}");
            }
        }

        public static void Deinitialize()
        {
            try
            {
                ModSettingsEvents.OnPreferencesSaved -= ApplySettings;
            }
            catch { }
        }
    }
}

