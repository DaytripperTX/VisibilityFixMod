using UnityEngine;
using System.Collections.Generic;
using static MelonLoader.MelonLogger;

namespace VisibilityFixMod
{
    public static class DebugUtil
    {
        public static void Log(string key, string message, string level = "debug", float cooldown = 1f)
        {
            if (!Config.EnableDebugLogs || !LogRateLimiter.CanLog(key, cooldown))
                return;

            switch (level.ToLowerInvariant())
            {
                case "warning":
                    Warning(message);
                    break;
                case "error":
                    Error(message);
                    break;
                default:
                    Msg(message);
                    break;
            }
        }

        public static void LogBlock(string key, string header, IEnumerable<string> lines, float cooldown = 1f)
        {
            if (!Config.EnableDebugLogs || !LogRateLimiter.CanLog(key, cooldown))
                return;

            Msg(header);
            foreach (var line in lines)
                Msg(line);
        }
    }


    public static class LogRateLimiter
    {
        private static readonly Dictionary<string, float> lastLogTimes = new Dictionary<string, float>();

        public static bool CanLog(string key, float cooldownSeconds)
        {
            float currentTime = Time.time;
            if (!lastLogTimes.TryGetValue(key, out float lastTime) || currentTime - lastTime >= cooldownSeconds)
            {
                lastLogTimes[key] = currentTime;
                return true;
            }
            return false;
        }
    }
}
