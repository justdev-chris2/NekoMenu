using System;
using System.Linq;
using UnityEngine;
using AmongUs.GameOptions;

namespace NekoMenu
{
    public class NekoMenuUI : MonoBehaviour
    {
        private bool menuVisible = false;
        private float menuX = 100f;
        private float menuY = 100f;
        private Rect menuRect;
        private Vector2 scrollPosition;
        private int selectedTab = 0;
        private string[] tabs = { "ESP", "KILL", "ROLES", "MOVEMENT", "ANIM" };
        
        private void Start()
        {
            menuRect = new Rect(menuX, menuY, 400, 500);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
                menuVisible = !menuVisible;
                
            // Run cheats every frame
            if (PlayerControl.LocalPlayer != null)
            {
                NekoCheats.NoKillCdCheat(PlayerControl.LocalPlayer);
                NekoCheats.UseVentCheat(HudManager.Instance);
                NekoCheats.WalkInVentCheat();
                NekoCheats.NoClipCheat();
                NekoCheats.TeleportCursorCheat();
                
                // Role-specific handlers
                if (PlayerControl.LocalPlayer.Data.Role is EngineerRole engineer)
                    NekoCheats.HandleEngineerCheats(engineer);
                    
                if (PlayerControl.LocalPlayer.Data.Role is ShapeshifterRole shapeshifter)
                    NekoCheats.HandleShapeshifterCheats(shapeshifter);
                    
                if (PlayerControl.LocalPlayer.Data.Role is ScientistRole scientist)
                    NekoCheats.HandleScientistCheats(scientist);
                    
                if (PlayerControl.LocalPlayer.Data.Role is TrackerRole tracker)
                    NekoCheats.HandleTrackerCheats(tracker);
            }
            
            // One-time cheats
            NekoCheats.CloseMeetingCheat();
            NekoCheats.SkipMeetingCheat();
            NekoCheats.CallMeetingCheat();
            NekoCheats.ForceStartGameCheat();
            NekoCheats.CompleteMyTasksCheat();
            NekoCheats.OpenSabotageMapCheat();
            NekoCheats.KickVentsCheat();
            NekoCheats.KillAllCheat();
            NekoCheats.KillAllCrewCheat();
            NekoCheats.KillAllImpsCheat();
            NekoCheats.ProtectCheat();
            NekoCheats.ReviveCheat();
        }
        
        private void OnGUI()
        {
            // Draw ESP
            if (CheatToggles.espEnabled)
            {
                DrawESP();
            }
            
            // Draw menu
            if (menuVisible)
            {
                menuRect = GUI.Window(0, menuRect, new GUI.WindowFunction(DrawMenu), "NEKO MENU (Right Ctrl)");
            }
        }
        
        private void DrawMenu(int windowID)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            
            // Tabs
            GUILayout.BeginHorizontal();
            for (int i = 0; i < tabs.Length; i++)
            {
                GUI.backgroundColor = (selectedTab == i) ? new Color(1f, 0.5f, 0.8f) : Color.gray;
                if (GUILayout.Button(tabs[i], GUILayout.Height(30)))
                    selectedTab = i;
            }
            GUILayout.EndHorizontal();
            
            GUI.backgroundColor = Color.gray;
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
            
            switch (selectedTab)
            {
                case 0: DrawEspTab(); break;
                case 1: DrawKillTab(); break;
                case 2: DrawRolesTab(); break;
                case 3: DrawMovementTab(); break;
                case 4: DrawAnimTab(); break;
            }
            
            GUILayout.EndScrollView();
        }
        
        private void DrawEspTab()
        {
            GUILayout.Label("ESP SETTINGS", GUI.skin.box);
            
            CheatToggles.espEnabled = GUILayout.Toggle(CheatToggles.espEnabled, "Enable ESP");
            CheatToggles.showGhosts = GUILayout.Toggle(CheatToggles.showGhosts, "Show Ghosts");
            CheatToggles.showImpostors = GUILayout.Toggle(CheatToggles.showImpostors, "Show Impostors");
            CheatToggles.showCrewmates = GUILayout.Toggle(CheatToggles.showCrewmates, "Show Crewmates");
            CheatToggles.showNames = GUILayout.Toggle(CheatToggles.showNames, "Show Names");
            CheatToggles.showRoles = GUILayout.Toggle(CheatToggles.showRoles, "Show Roles");
            CheatToggles.showDistance = GUILayout.Toggle(CheatToggles.showDistance, "Show Distance");
        }
        
        private void DrawKillTab()
        {
            GUILayout.Label("KILL OPTIONS", GUI.skin.box);
            
            CheatToggles.zeroKillCd = GUILayout.Toggle(CheatToggles.zeroKillCd, "No Kill Cooldown");
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Kill All Players", GUILayout.Height(30)))
            {
                CheatToggles.killAll = true;
            }
            
            if (GUILayout.Button("Kill All Crewmates", GUILayout.Height(30)))
            {
                CheatToggles.killAllCrew = true;
            }
            
            if (GUILayout.Button("Kill All Impostors", GUILayout.Height(30)))
            {
                CheatToggles.killAllImps = true;
            }
            
            GUILayout.Space(10);
            
            GUILayout.Label("PLAYER LIST", GUI.skin.box);
            
            if (PlayerControl.AllPlayerControls != null)
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player == null || player == PlayerControl.LocalPlayer || player.Data == null) continue;
                    
                    string status = player.Data.IsDead ? "💀" : "❤️";
                    string role = player.Data.Role != null ? player.Data.Role.ToString() : "No Role";
                    
                    if (GUILayout.Button($"{status} {player.Data.PlayerName} - {role}"))
                    {
                        // Toggle protect for this player
                        if (CheatToggles.playersToProtect.Contains(player))
                            CheatToggles.playersToProtect.Remove(player);
                        else
                            CheatToggles.playersToProtect.Add(player);
                    }
                }
            }
        }
        
        private void DrawRolesTab()
        {
            GUILayout.Label("ENGINEER", GUI.skin.box);
            CheatToggles.endlessVentTime = GUILayout.Toggle(CheatToggles.endlessVentTime, "Endless Vent Time");
            CheatToggles.noVentCooldown = GUILayout.Toggle(CheatToggles.noVentCooldown, "No Vent Cooldown");
            CheatToggles.useVents = GUILayout.Toggle(CheatToggles.useVents, "Use Vents (Any Role)");
            CheatToggles.walkVent = GUILayout.Toggle(CheatToggles.walkVent, "Walk in Vents");
            
            GUILayout.Space(10);
            
            GUILayout.Label("SHAPESHIFTER", GUI.skin.box);
            CheatToggles.endlessSsDuration = GUILayout.Toggle(CheatToggles.endlessSsDuration, "Endless Shapeshift");
            
            GUILayout.Space(10);
            
            GUILayout.Label("SCIENTIST", GUI.skin.box);
            CheatToggles.noVitalsCooldown = GUILayout.Toggle(CheatToggles.noVitalsCooldown, "No Vitals Cooldown");
            CheatToggles.endlessBattery = GUILayout.Toggle(CheatToggles.endlessBattery, "Endless Battery");
            
            GUILayout.Space(10);
            
            GUILayout.Label("TRACKER", GUI.skin.box);
            CheatToggles.noTrackingCooldown = GUILayout.Toggle(CheatToggles.noTrackingCooldown, "No Tracking Cooldown");
            CheatToggles.noTrackingDelay = GUILayout.Toggle(CheatToggles.noTrackingDelay, "No Tracking Delay");
            CheatToggles.endlessTracking = GUILayout.Toggle(CheatToggles.endlessTracking, "Endless Tracking");
        }
        
        private void DrawMovementTab()
        {
            GUILayout.Label("MOVEMENT", GUI.skin.box);
            
            CheatToggles.noClip = GUILayout.Toggle(CheatToggles.noClip, "No Clip");
            CheatToggles.teleportCursor = GUILayout.Toggle(CheatToggles.teleportCursor, "Teleport to Cursor (Right Click)");
            
            GUILayout.Space(10);
            
            GUILayout.Label("MEETINGS", GUI.skin.box);
            
            if (GUILayout.Button("Close Meeting", GUILayout.Height(25)))
                CheatToggles.closeMeeting = true;
                
            if (GUILayout.Button("Skip Meeting", GUILayout.Height(25)))
                CheatToggles.skipMeeting = true;
                
            if (GUILayout.Button("Call Meeting", GUILayout.Height(25)))
                CheatToggles.callMeeting = true;
                
            if (GUILayout.Button("Force Start Game", GUILayout.Height(25)))
                CheatToggles.forceStartGame = true;
                
            GUILayout.Space(10);
            
            if (GUILayout.Button("Open Sabotage Map", GUILayout.Height(25)))
                CheatToggles.sabotageMap = true;
                
            if (GUILayout.Button("Kick All Vents", GUILayout.Height(25)))
                CheatToggles.kickVents = true;
                
            if (GUILayout.Button("Complete My Tasks", GUILayout.Height(25)))
                CheatToggles.completeMyTasks = true;
                
            if (GUILayout.Button("Revive (Fake)", GUILayout.Height(25)))
                CheatToggles.fakeRevive = true;
        }
        
        private void DrawAnimTab()
        {
            GUILayout.Label("ANIMATIONS", GUI.skin.box);
            GUILayout.Label("Animation cheats removed - API changed", GUI.skin.label);
        }
        
        private void DrawESP()
        {
            if (PlayerControl.LocalPlayer == null || Camera.main == null) return;
            
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player == null || player == PlayerControl.LocalPlayer || player.Data == null) continue;
                
                bool isGhost = player.Data.IsDead;
                bool isImpostor = player.Data.Role != null && player.Data.Role.IsImpostor;
                
                // Filter logic
                if (isGhost && !CheatToggles.showGhosts) continue;
                if (isImpostor && !CheatToggles.showImpostors) continue;
                if (!isImpostor && !isGhost && !CheatToggles.showCrewmates) continue;
                
                Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
                screenPos.y = Screen.height - screenPos.y;
                
                if (screenPos.z > 0)
                {
                    Color espColor = isGhost ? Color.gray : (isImpostor ? Color.red : Color.green);
                    float distance = Vector3.Distance(PlayerControl.LocalPlayer.transform.position, player.transform.position);
                    
                    // Draw box
                    DrawBox(screenPos, 50, 65, espColor, 2f);
                    
                    string info = "";
                    if (CheatToggles.showNames)
                        info += player.Data.PlayerName + "\n";
                    if (CheatToggles.showRoles)
                        info += (player.Data.Role?.ToString() ?? "No Role") + "\n";
                    if (CheatToggles.showDistance)
                        info += $"{distance:F1}m";
                    
                    if (!string.IsNullOrEmpty(info))
                    {
                        GUI.color = espColor;
                        GUI.Label(new Rect(screenPos.x - 75, screenPos.y - 50, 150, 60), info);
                        GUI.color = Color.white;
                    }
                }
            }
        }
        
        private void DrawBox(Vector3 pos, float width, float height, Color color, float thickness)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            
            // Top
            GUI.DrawTexture(new Rect(pos.x - width/2, pos.y - height/2, width, thickness), texture);
            // Bottom
            GUI.DrawTexture(new Rect(pos.x - width/2, pos.y + height/2 - thickness, width, thickness), texture);
            // Left
            GUI.DrawTexture(new Rect(pos.x - width/2, pos.y - height/2, thickness, height), texture);
            // Right
            GUI.DrawTexture(new Rect(pos.x + width/2 - thickness, pos.y - height/2, thickness, height), texture);
        }
    }
}
