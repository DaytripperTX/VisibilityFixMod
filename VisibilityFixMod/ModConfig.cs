using System.IO;
using System.Reflection;
using static MelonLoader.MelonLogger;
using Newtonsoft.Json;
using MelonLoader;
using MelonLoader.Utils;
using System;
using System.Linq;

namespace VisibilityFixMod
{
    public static class Config
    {
        private static readonly string configPath = Path.Combine(MelonEnvironment.UserDataDirectory, "VisibilityFixConfig.json");

        public static bool EnableDebugLogs = false;
        public static bool FlashlightAffectsSneak = true;
        public static float BaseVisibility = 60f;
        public static float MaxVisibility = 60f;

        public static MultiplierSettings[] Multipliers = new[] { new MultiplierSettings() };

        public static void Load()
        {
            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                var cfg = JsonConvert.DeserializeObject<ConfigData>(json);

                EnableDebugLogs = cfg.EnableDebugLogs;
                FlashlightAffectsSneak = cfg.FlashlightAffectsSneak;
                BaseVisibility = cfg.BaseVisibility;
                MaxVisibility = cfg.MaxVisibility;
                Multipliers = cfg.Multipliers ?? new[] { new MultiplierSettings() };
            }
            else
            {
                Save(); // create file on first run
            }

            bool isModManagerAvailable = AppDomain.CurrentDomain.GetAssemblies()
            .Any(assembly => assembly.GetName().Name == "ModManager&PhoneApp");

            if (isModManagerAvailable || true)
            {
                var category = MelonPreferences.CreateCategory("VisibilityFix", "Main Settings");
                category.CreateEntry("EnableDebugLogs", Config.EnableDebugLogs);
                category.CreateEntry("FlashlightAffectsSneak", Config.FlashlightAffectsSneak);
                category.CreateEntry("BaseVisibility", Config.BaseVisibility);
                category.CreateEntry("MaxVisibility", Config.MaxVisibility);
                // Add entries for Multipliers later


            }
        }

        public static void HandleSettingsUpdate()
        {
            try
            {
                // Reload values from ModManager entries
                EnableDebugLogs = MelonPreferences.GetEntryValue<bool>("VisibilityFix", "EnableDebugLogs");
                FlashlightAffectsSneak = MelonPreferences.GetEntryValue<bool>("VisibilityFix", "FlashlightAffectsSneak");
                BaseVisibility = MelonPreferences.GetEntryValue<float>("VisibilityFix", "BaseVisibility");
                MaxVisibility = MelonPreferences.GetEntryValue<float>("VisibilityFix", "MaxVisibility");

                // Log (optional)
                Msg("[DEBUG] Settings updated via Mod Manager.");

                // Save to config file
                Save();
            }
            catch (Exception ex)
            {
                Msg($"[ERROR] Failed to apply updated settings: {ex.Message}");
            }
        }

        public static void Save()
        {
            var data = new ConfigData
            {
                EnableDebugLogs = EnableDebugLogs,
                FlashlightAffectsSneak = FlashlightAffectsSneak,
                BaseVisibility = BaseVisibility,
                MaxVisibility = MaxVisibility,
                Multipliers = Multipliers
            };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            Msg("[DEBUG] JSON:\n" + json);
            File.WriteAllText(configPath, json);
        }
        public static MultiplierSettings ActiveMultipliers => Multipliers.Length > 0 ? Multipliers[0] : new MultiplierSettings();
    }

    public class MultiplierSettings
    {
        public float Sneaky = 0.6f;
        public float Crouched = 0.8f;
        public float Flashlight = 1.5f;
    }

    public class ConfigData
    {
        public bool EnableDebugLogs = false;
        public bool FlashlightAffectsSneak = true;
        public float BaseVisibility = 60f;
        public float MaxVisibility = 60f;

        public MultiplierSettings[] Multipliers = new[] { new MultiplierSettings() };
    }

}
