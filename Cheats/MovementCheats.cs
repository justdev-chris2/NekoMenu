using UnityEngine;

namespace NekoMenu
{
    public static class MovementCheats
    {
        public static void NoClipCheat()
        {
            try
            {
                PlayerControl.LocalPlayer.Collider.enabled = !(CheatToggles.noClip || PlayerControl.LocalPlayer.onLadder);
            } catch { }
        }

        public static void TeleportCursorCheat()
        {
            if (!CheatToggles.teleportCursor) return;
            if (Input.GetMouseButtonDown(1))
                PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        public static void SpeedHackCheat()
        {
            if (!CheatToggles.speedHackEnabled || PlayerControl.LocalPlayer == null) return;
            var physics = PlayerControl.LocalPlayer.MyPhysics;
            if (physics != null)
                physics.Speed = CheatToggles.speedMultiplier;
        }

        public static void ReviveCheat()
        {
            if (!CheatToggles.fakeRevive) return;
            PlayerControl.LocalPlayer.Revive();
            CheatToggles.fakeRevive = false;
        }
    }
}
