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

        public static void SabotageCheat()
        {
            if (PlayerControl.LocalPlayer == null || ShipStatus.Instance == null) return;
            
            if (CheatToggles.sabotageLights)
            {
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Electrical, 1);
                CheatToggles.sabotageLights = false;
            }
            if (CheatToggles.sabotageComms)
            {
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Comms, 1);
                CheatToggles.sabotageComms = false;
            }
            if (CheatToggles.sabotageO2)
            {
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.LifeSupp, 1);
                CheatToggles.sabotageO2 = false;
            }
            if (CheatToggles.sabotageReactor)
            {
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Reactor, 1);
                CheatToggles.sabotageReactor = false;
            }
            if (CheatToggles.sabotageHeli)
            {
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.HeliSabotage, 1);
                CheatToggles.sabotageHeli = false;
            }
        }
    }
}
