using System;
using System.Linq;
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
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead)
                        PlayerControl.LocalPlayer.MurderPlayer(player, MurderResultFlags.Succeeded);
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
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead && player.Data.Role.TeamType == RoleTeamTypes.Crewmate)
                        PlayerControl.LocalPlayer.MurderPlayer(player, MurderResultFlags.Succeeded);
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
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead && player.Data.Role.TeamType == RoleTeamTypes.Impostor)
                        PlayerControl.LocalPlayer.MurderPlayer(player, MurderResultFlags.Succeeded);
                }
            }
            CheatToggles.killAllImps = false;
        }

        public static void KillAllLobbyCheat()
        {
            if (!CheatToggles.killAllLobby) return;

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player != null && player.Data != null && !player.Data.IsDead)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                        PlayerControl.LocalPlayer.NetId,
                        (byte)RpcCalls.MurderPlayer,
                        SendOption.Reliable,
                        -1
                    );
                    writer.WriteNetObject(player);
                    writer.Write((int)MurderResultFlags.Succeeded);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    
                    PlayerControl.LocalPlayer.MurderPlayer(player, MurderResultFlags.Succeeded);
                }
            }
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
    }
}
