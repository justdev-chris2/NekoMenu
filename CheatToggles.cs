using System.Collections.Generic;
using UnityEngine;

namespace NekoMenu
{
    public static class CheatToggles
    {
        // ESP Toggles
        public static bool espEnabled = true;
        public static bool showGhosts = true;
        public static bool showImpostors = true;
        public static bool showCrewmates = true;
        public static bool showNames = true;
        public static bool showRoles = true;
        public static bool showDistance = true;
        
        // Player toggles
        public static bool killAll = false;
        public static bool killAllCrew = false;
        public static bool killAllImps = false;
        public static bool teleportCursor = false;
        public static bool noClip = false;
        public static bool fakeRevive = false;
        
        // Role toggles
        public static bool endlessVentTime = false;
        public static bool noVentCooldown = false;
        public static bool endlessSsDuration = false;
        public static bool noVitalsCooldown = false;
        public static bool endlessBattery = false;
        public static bool noTrackingCooldown = false;
        public static bool noTrackingDelay = false;
        public static bool endlessTracking = false;
        public static bool useVents = false;
        public static bool walkVent = false;
        public static bool kickVents = false;
        
        // Kill toggles
        public static bool zeroKillCd = false;
        
        // Meeting toggles
        public static bool closeMeeting = false;
        public static bool skipMeeting = false;
        public static bool callMeeting = false;
        public static bool forceStartGame = false;
        public static bool sabotageMap = false;
        
        // Tasks
        public static bool completeMyTasks = false;
        
        // List for protect cheat
        public static List<PlayerControl> playersToProtect = new List<PlayerControl>();
    }
}
