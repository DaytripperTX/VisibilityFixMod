using MelonLoader;
using ModManagerPhoneApp;
using static MelonLoader.MelonLogger;

namespace VisibilityFixMod
{
    public static class ModManagerIntegration
    {
        private static MelonPreferences_Category mainCategory;
        private static MelonPreferences_Category multiplierCategory;
        private static bool initialized = false;

        public static void Initialize()
        {
            if (initialized)
                return;

            try
            {
                mainCategory = MelonPreferences.CreateCategory("VisibilityFix", "Main Settings");
                mainCategory.CreateEntry("EnableDebugLogs", Config.EnableDebugLogs);
                mainCategory.CreateEntry("FlashlightAffectsSneak", Config.FlashlightAffectsSneak);
                mainCategory.CreateEntry("BaseVisibility", Config.BaseVisibility);
                mainCategory.CreateEntry("MaxVisibility", Config.MaxVisibility);

                multiplierCategory = MelonPreferences.CreateCategory("VisibilityFix_Multipliers", "Multiplier Settings");
                multiplierCategory.CreateEntry("Sneaky", Config.ActiveMultipliers.Sneaky);
                multiplierCategory.CreateEntry("Crouched", Config.ActiveMultipliers.Crouched);
                multiplierCategory.CreateEntry("Flashlight", Config.ActiveMultipliers.Flashlight);

                ModSettingsEvents.OnPreferencesSaved += ApplySettings;
                initialized = true;
                Msg("[ModManagerIntegration] Initialized successfully.");
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
                Config.EnableDebugLogs = mainCategory.GetEntry<bool>("EnableDebugLogs").Value;
                Config.FlashlightAffectsSneak = mainCategory.GetEntry<bool>("FlashlightAffectsSneak").Value;
                Config.BaseVisibility = mainCategory.GetEntry<float>("BaseVisibility").Value;
                Config.MaxVisibility = mainCategory.GetEntry<float>("MaxVisibility").Value;

                var multipliers = Config.ActiveMultipliers;
                multipliers.Sneaky = multiplierCategory.GetEntry<float>("Sneaky").Value;
                multipliers.Crouched = multiplierCategory.GetEntry<float>("Crouched").Value;
                multipliers.Flashlight = multiplierCategory.GetEntry<float>("Flashlight").Value;

                Config.Multipliers[0] = multipliers;

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

