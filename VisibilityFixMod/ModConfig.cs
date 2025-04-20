using System.IO;
using System.Reflection;
using static MelonLoader.MelonLogger;
using Newtonsoft.Json;

namespace VisibilityFixMod
{
    public static class Config
    {
        private static readonly string ModDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string configPath = Path.Combine(ModDirectory, "VisibilityFixConfig.json");

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
