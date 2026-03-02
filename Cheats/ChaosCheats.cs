using System;
using System.Linq;
using Hazel;
using InnerNet;
using UnityEngine;

namespace NekoMenu
{
    public static class ChaosCheats
    {
        public static void SendCustomNotification()
        {
            if (!CheatToggles.sendCustomNotification || string.IsNullOrEmpty(CheatToggles.customNotificationText)) return;
            
            string message = CheatToggles.customNotificationText;
            
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                PlayerControl.LocalPlayer.NetId,
                (byte)RpcCalls.SetNotifier,
                SendOption.Reliable,
                -1
            );
            writer.Write(message);
            writer.Write(5f);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            
            if (HudManager.Instance != null)
                HudManager.Instance.Notifier.AddItem(message);
            
            CheatToggles.sendCustomNotification = false;
        }

        public static void FakeReportCheat()
        {
            if (!CheatToggles.fakeReport || CheatToggles.fakeReportTargetId < 0) return;
            
            var target = PlayerControl.AllPlayerControls.ToArray()
                .FirstOrDefault(p => p != null && p.PlayerId == CheatToggles.fakeReportTargetId);
            
            if (target != null)
            {
                string reportMsg = $"{target.Data.PlayerName} reported a body";
                
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                    PlayerControl.LocalPlayer.NetId,
                    (byte)RpcCalls.SetNotifier,
                    SendOption.Reliable,
                    -1
                );
                writer.Write(reportMsg);
                writer.Write(4f);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                
                HudManager.Instance.Notifier.AddItem(reportMsg);
            }
            
            CheatToggles.fakeReport = false;
        }

        public static void TeleportAllToMeCheat()
        {
            if (!CheatToggles.teleportAllToMe || PlayerControl.LocalPlayer == null) return;
            
            Vector2 myPos = PlayerControl.LocalPlayer.transform.position;
            
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player != null && player != PlayerControl.LocalPlayer)
                {
                    player.NetTransform.RpcSnapTo(myPos);
                }
            }
            
            CheatToggles.teleportAllToMe = false;
        }

        public static void FreezeAllCheat()
        {
            if (!CheatToggles.freezeAll || PlayerControl.LocalPlayer == null) return;
            
            // Your existing freeze code here
            // (whatever you had for freeze)
            
            CheatToggles.freezeAll = false;
        }

        public static void FakeMeetingFlashCheat()
        {
            if (!CheatToggles.fakeMeetingFlash || HudManager.Instance == null) return;
            
            HudManager.Instance.ReportButton.flashAlpha = 1f;
            HudManager.Instance.ReportButton.StartFlash(0.5f);
            
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                PlayerControl.LocalPlayer.NetId,
                (byte)RpcCalls.SetNotifier,
                SendOption.Reliable,
                -1
            );
            writer.Write("Emergency meeting button pressed");
            writer.Write(3f);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            
            HudManager.Instance.Notifier.AddItem("Emergency meeting button pressed");
            
            CheatToggles.fakeMeetingFlash = false;
        }

        public static void FakeDeathScreenCheat()
        {
            if (!CheatToggles.fakeDeathScreen || CheatToggles.fakeDeathTargetId < 0) return;
            
            var target = PlayerControl.AllPlayerControls.ToArray()
                .FirstOrDefault(p => p != null && p.PlayerId == CheatToggles.fakeDeathTargetId);
            
            if (target != null)
            {
                string deathMsg = $"{target.Data.PlayerName} died";
                
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                    PlayerControl.LocalPlayer.NetId,
                    (byte)RpcCalls.SetNotifier,
                    SendOption.Reliable,
                    -1
                );
                writer.Write(deathMsg);
                writer.Write(4f);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                
                HudManager.Instance.Notifier.AddItem(deathMsg);
                
                if (target == PlayerControl.LocalPlayer)
                {
                    HudManager.Instance.FullScreen.color = new Color(1f, 0f, 0f, 0.5f);
                    HudManager.Instance.StartCoroutine(HudManager.Instance.CoFadeFullScreen(Color.red, Color.clear, 0.5f, false));
                }
            }
            
            CheatToggles.fakeDeathScreen = false;
        }

        public static void FakeWinScreenCheat()
        {
            if (!CheatToggles.fakeWinScreen) return;
            
            string winMsg = CheatToggles.fakeWinTeam == 0 ? "Crewmates Win!" : "Impostors Win!";
            
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                PlayerControl.LocalPlayer.NetId,
                (byte)RpcCalls.SetNotifier,
                SendOption.Reliable,
                -1
            );
            writer.Write(winMsg);
            writer.Write(5f);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            
            HudManager.Instance.Notifier.AddItem(winMsg);
            
            CheatToggles.fakeWinScreen = false;
        }
    }
}
