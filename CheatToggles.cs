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
        
        // Sabotage toggles
        public static bool sabotageLights = false;
        public static bool sabotageComms = false;
        public static bool sabotageO2 = false;
        public static bool sabotageReactor = false;
        public static bool sabotageHeli = false;
        
        // Chaos toggles
        public static string customNotificationText = "";
        public static bool sendCustomNotification = false;
        public static bool fakeReport = false;
        public static int fakeReportTargetId = -1;
        public static bool teleportAllToMe = false;
        public static bool freezeAll = false;
        public static bool fakeMeetingFlash = false;
        public static bool fakeDeathScreen = false;
        public static int fakeDeathTargetId = -1;
        public static bool fakeWinScreen = false;
        public static int fakeWinTeam = 0;
        
        // Protect list
        public static List<PlayerControl> playersToProtect = new List<PlayerControl>();
    }
}
