using System;
using System.Linq;
using System.Threading;
using Hazel;
using InnerNet;
using UnityEngine;

namespace NekoMenu
{
    public static class ChaosCheats
    {
        public static void TeleportAllToMeCheat()
        {
            if (!CheatToggles.teleportAllToMe || PlayerControl.LocalPlayer == null) return;
            
            Vector2 myPos = PlayerControl.LocalPlayer.transform.position;
            int count = 0;
            
            HudManager.Instance.Notifier.AddDisconnectMessage("Teleporting players slowly...");
            
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player != null && player != PlayerControl.LocalPlayer)
                {
                    player.NetTransform.RpcSnapTo(myPos);
                    count++;
                    
                    if (count % 2 == 0)
                        Thread.Sleep(100);
                }
            }
            
            HudManager.Instance.Notifier.AddDisconnectMessage($"Teleported {count} players");
            CheatToggles.teleportAllToMe = false;
        }

        public static void FreezeAllCheat()
        {
            if (!CheatToggles.freezeAll || PlayerControl.LocalPlayer == null) return;
            
            int count = 0;
            HudManager.Instance.Notifier.AddDisconnectMessage("Freezing players...");
            
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player != null && player != PlayerControl.LocalPlayer)
                {
                    // Freeze by setting position repeatedly
                    Vector3 frozenPos = player.transform.position;
                    
                    // Send RPC to lock their position
                    for (int i = 0; i < 5; i++)
                    {
                        player.NetTransform.RpcSnapTo(frozenPos);
                        Thread.Sleep(10);
                    }
                    
                    count++;
                    
                    if (count % 2 == 0)
                        Thread.Sleep(100);
                }
            }
            
            HudManager.Instance.Notifier.AddDisconnectMessage($"Froze {count} players");
            CheatToggles.freezeAll = false;
        }

        public static void NoChatCooldownCheat()
        {
            if (!CheatToggles.noChatCooldown || PlayerControl.LocalPlayer == null || HudManager.Instance == null) return;
            
            if (HudManager.Instance.Chat != null)
            {
                HudManager.Instance.Chat.timeSinceLastMessage = 999f;
            }
        }
    }
}
