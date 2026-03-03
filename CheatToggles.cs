using System.Collections.Generic;
using UnityEngine;

namespace NekoMenu
{
    public static class CheatToggles
    {
        // ESP Toggles
        public static bool espEnabled = false;
        public static bool showGhosts = true;
        public static bool showImpostors = true;
        public static bool showCrewmates = true;
        public static bool showNames = true;
        public static bool showRoles = true;
        public static bool showDistance = true;
        
        // Kill toggles
        public static bool zeroKillCd = false;
        public static int selectedTargetId = -1;
        public static bool killSelected = false;
        public static bool killSelf = false;
        public static bool killAll = false;
        public static bool killAllCrew = false;
        public static bool killAllImps = false;
        public static bool killAllLobby = false;
        public static bool reviveSelected = false;
        public static int reviveTargetId = -1;
        
        // Role toggles
        public static bool endlessVentTime = false;
        public static bool noVentCooldown = false;
        public static bool useVents = false;
        public static bool walkVent = false;
        public static bool kickVents = false;
        public static bool endlessSsDuration = false;
        public static bool noVitalsCooldown = false;
        public static bool endlessBattery = false;
        public static bool noTrackingCooldown = false;
        public static bool noTrackingDelay = false;
        public static bool endlessTracking = false;
        
        // Role changer
        public static bool changeRole = false;
        public static int selectedRoleIndex = 0;
        
        // Meeting toggles
        public static bool closeMeeting = false;
        public static bool skipMeeting = false;
        public static bool callMeeting = false;
        public static bool forceStartGame = false;
        public static bool sabotageMap = false;
        public static bool completeMyTasks = false;
        
        // Movement toggles
        public static bool noClip = false;
        public static bool teleportCursor = false;
        public static bool speedHackEnabled = false;
        public static float speedMultiplier = 2f;
        public static bool fakeRevive = false;
    
        // Sabotage toggles (updated)
        public static bool reactorSab = false;
        public static bool oxygenSab = false;
        public static bool commsSab = false;
        public static bool elecSab = false;
        public static bool unfixableLights = false;
        public static bool mushSab = false;
        public static bool mushSpore = false;
        public static bool closeAllDoors = false;
        public static bool openAllDoors = false;
        public static bool spamCloseAllDoors = false;
        public static bool spamOpenAllDoors = false;
        
        // Chaos toggles
        public static bool teleportAllToMe = false;
        public static bool freezeAll = false;
        
        // Chat toggles
        public static bool noChatCooldown = false;
        
        // Protect list
        public static List<PlayerControl> playersToProtect = new List<PlayerControl>();
    }
}
