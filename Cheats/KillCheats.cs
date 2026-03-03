using System;
using System.Linq;
using System.Threading;
using Hazel;
using InnerNet;
using UnityEngine;

namespace NekoMenu
{
    public static class KillCheats
    {
        public static void NoKillCdCheat(PlayerControl playerControl)
        {
            if (CheatToggles.zeroKillCd && playerControl.killTimer > 0f)
                playerControl.SetKillTimer(0f);
        }

        public static void KillAllCheat()
        {
            if (!CheatToggles.killAll) return;

            if (Utils.isLobby)
            {
                HudManager.Instance.Notifier.AddDisconnectMessage("Killing in lobby disabled");
            }
            else
            {
                int count = 0;
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead)
                    {
                        PlayerControl.LocalPlayer.MurderPlayer(player, MurderResultFlags.Succeeded);
                        count++;
                        
                        if (count % 3 == 0)
                            Thread.Sleep(50);
                    }
                }
            }
            CheatToggles.killAll = false;
        }

        public static void KillAllCrewCheat()
        {
            if (!CheatToggles.killAllCrew) return;

            if (Utils.isLobby)
            {
                HudManager.Instance.Notifier.AddDisconnectMessage("Killing in lobby disabled");
            }
            else
            {
                int count = 0;
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead && player.Data.Role.TeamType == RoleTeamTypes.Crewmate)
                    {
                        PlayerControl.LocalPlayer.MurderPlayer(player, MurderResultFlags.Succeeded);
                        count++;
                        
                        if (count % 3 == 0)
                            Thread.Sleep(50);
                    }
                }
            }
            CheatToggles.killAllCrew = false;
        }

        public static void KillAllImpsCheat()
        {
            if (!CheatToggles.killAllImps) return;

            if (Utils.isLobby)
            {
                HudManager.Instance.Notifier.AddDisconnectMessage("Killing in lobby disabled");
            }
            else
            {
                int count = 0;
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead && player.Data.Role.TeamType == RoleTeamTypes.Impostor)
                    {
                        PlayerControl.LocalPlayer.MurderPlayer(player, MurderResultFlags.Succeeded);
                        count++;
                        
                        if (count % 3 == 0)
                            Thread.Sleep(50);
                    }
                }
            }
            CheatToggles.killAllImps = false;
        }

        public static void KillAllLobbyCheat()
        {
            if (!CheatToggles.killAllLobby) return;

            HudManager.Instance.Notifier.AddDisconnectMessage("Nuking lobby...");
            int count = 0;
            
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player != null)
                {
                    AmongUsClient.Instance.KickPlayer(player.OwnerId, false);
                    count++;
                    
                    if (count % 2 == 0)
                        Thread.Sleep(150);
                }
            }
            
            Thread.Sleep(500);
            AmongUsClient.Instance.ExitGame(DisconnectReasons.ExitGame);
            
            CheatToggles.killAllLobby = false;
        }

        public static void KillSelectedCheat()
        {
            if (!CheatToggles.killSelected || CheatToggles.selectedTargetId < 0) return;
            
            var target = PlayerControl.AllPlayerControls.ToArray()
                .FirstOrDefault(p => p != null && p.PlayerId == CheatToggles.selectedTargetId);
            
            if (target != null)
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                    PlayerControl.LocalPlayer.NetId,
                    (byte)RpcCalls.MurderPlayer,
                    SendOption.Reliable,
                    -1
                );
                writer.WriteNetObject(target);
                writer.Write((int)MurderResultFlags.Succeeded);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                
                PlayerControl.LocalPlayer.MurderPlayer(target, MurderResultFlags.Succeeded);
                Utils.ShowMessage($"Killed {target.Data.PlayerName}");
            }
            CheatToggles.killSelected = false;
        }

        public static void KillSelfCheat()
        {
            if (!CheatToggles.killSelf || PlayerControl.LocalPlayer == null) return;
            
            PlayerControl.LocalPlayer.MurderPlayer(PlayerControl.LocalPlayer, MurderResultFlags.Succeeded);
            CheatToggles.killSelf = false;
        }

        public static void ReviveSelectedCheat()
{
    if (!CheatToggles.reviveSelected || CheatToggles.reviveTargetId < 0) return;
    
    var target = PlayerControl.AllPlayerControls.ToArray()
        .FirstOrDefault(p => p != null && p.PlayerId == CheatToggles.reviveTargetId);
    
    if (target != null && target.Data.IsDead)
    {
        // Use MurderPlayer RPC with Succeeded flag? No that kills.
        // Just use the local method - it actually works
        target.Revive();
        target.Data.IsDead = false;
        
        // Force sync via RPC (use a different approach)
        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
            PlayerControl.LocalPlayer.NetId,
            (byte)RpcCalls.CheckMurder, // Use a generic RPC to sync
            SendOption.Reliable,
            -1
        );
        writer.WriteNetObject(target);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        
        Utils.ShowMessage($"Revived {target.Data.PlayerName}");
    }
    
    CheatToggles.reviveSelected = false;
}
    }
}
