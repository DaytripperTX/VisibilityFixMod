using UnityEngine;
using System.Collections.Generic;

namespace VisibilityFixMod
{
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
