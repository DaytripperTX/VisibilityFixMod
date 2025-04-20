using System.IO;
using System.Reflection;
using UnityEngine;

namespace VisibilityFixMod
{
    public static class Config
    {
        private static readonly string ModDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string configPath = Path.Combine(ModDirectory, "VisibilityFixConfig.json");

        public static float MaxVisibility = 30f; // default

        public static void Load()
        {
            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                var cfg = JsonUtility.FromJson<ConfigData>(json);
                MaxVisibility = cfg.MaxVisibility;
            }
            else
            {
                Save(); // create file on first run
            }
        }

        public static void Save()
        {
            var data = new ConfigData { MaxVisibility = MaxVisibility };
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(configPath, json);
        }

        [System.Serializable]
        public class ConfigData
        {
            public float MaxVisibility = 30f;
        }
    }
}
