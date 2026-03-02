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
            Utils.SendNotification(CheatToggles.customNotificationText, 5f);
            CheatToggles.sendCustomNotification = false;
        }

        public static void FakeReportCheat()
        {
            if (!CheatToggles.fakeReport || CheatToggles.fakeReportTargetId < 0) return;
            
            var target = PlayerControl.AllPlayerControls.ToArray()
                .FirstOrDefault(p => p != null && p.PlayerId == CheatToggles.fakeReportTargetId);
            
            if (target != null)
                Utils.SendNotification($"{target.Data.PlayerName} reported a body", 4f);
            
            CheatToggles.fakeReport = false;
        }

        public static void TeleportAllToMeCheat()
        {
            if (!CheatToggles.teleportAllToMe || PlayerControl.LocalPlayer == null) return;
            
            Vector2 myPos = PlayerControl.LocalPlayer.transform.position;
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player != null && player != PlayerControl.LocalPlayer)
                    player.NetTransform.RpcSnapTo(myPos);
            }
            CheatToggles.teleportAllToMe = false;
        }

        public static void FakeMeetingFlashCheat()
        {
            if (!CheatToggles.fakeMeetingFlash || HudManager.Instance == null) return;
            
            HudManager.Instance.ReportButton.flashAlpha = 1f;
            HudManager.Instance.ReportButton.StartFlash(0.5f);
            Utils.SendNotification("Emergency meeting button pressed", 3f);
            CheatToggles.fakeMeetingFlash = false;
        }

        public static void FakeDeathScreenCheat()
        {
            if (!CheatToggles.fakeDeathScreen || CheatToggles.fakeDeathTargetId < 0) return;
            
            var target = PlayerControl.AllPlayerControls.ToArray()
                .FirstOrDefault(p => p != null && p.PlayerId == CheatToggles.fakeDeathTargetId);
            
            if (target != null)
            {
                Utils.SendNotification($"{target.Data.PlayerName} died", 4f);
                
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
            Utils.SendNotification(winMsg, 5f);
            CheatToggles.fakeWinScreen = false;
        }
    }
}
