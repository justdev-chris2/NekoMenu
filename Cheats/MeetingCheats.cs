using AmongUs.GameOptions;
using InnerNet;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace NekoMenu
{
    public static class MeetingCheats
    {
        public static void CloseMeetingCheat()
        {
            if (!CheatToggles.closeMeeting) return;

            if (Utils.isMeeting)
            {
                MeetingHud.Instance.DespawnOnDestroy = false;
                Object.Destroy(MeetingHud.Instance.gameObject);

                DestroyableSingleton<HudManager>.Instance.StartCoroutine(DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.black, Color.clear, 0.2f, false));
                PlayerControl.LocalPlayer.SetKillTimer(GameManager.Instance.LogicOptions.GetKillCooldown());
                ShipStatus.Instance.EmergencyCooldown = GameManager.Instance.LogicOptions.GetEmergencyCooldown();
                Camera.main.GetComponent<FollowerCamera>().Locked = false;
                DestroyableSingleton<HudManager>.Instance.SetMapButtonEnabled(true);
                DestroyableSingleton<HudManager>.Instance.SetHudActive(true);
                ControllerManager.Instance.CloseAndResetAll();
            }
            else if (ExileController.Instance)
            {
                ExileController.Instance.ReEnableGameplay();
                ExileController.Instance.WrapUp();
            }
            CheatToggles.closeMeeting = false;
        }

        public static void SkipMeetingCheat()
        {
            if (!CheatToggles.skipMeeting) return;
            if (Utils.isMeeting)
                MeetingHud.Instance.RpcVotingComplete(new Il2CppStructArray<MeetingHud.VoterState>(0L), null, true);
            CheatToggles.skipMeeting = false;
        }

        public static void CallMeetingCheat()
        {
            if (!CheatToggles.callMeeting) return;

            if (Utils.isHost)
            {
                MeetingRoomManager.Instance.AssignSelf(PlayerControl.LocalPlayer, null);
                DestroyableSingleton<HudManager>.Instance.OpenMeetingRoom(PlayerControl.LocalPlayer);
                PlayerControl.LocalPlayer.RpcStartMeeting(null);
            }
            else
            {
                PlayerControl.LocalPlayer.CmdReportDeadBody(null);
            }
            CheatToggles.callMeeting = false;
        }

        public static void ForceStartGameCheat()
        {
            if (!CheatToggles.forceStartGame) return;

            if (Utils.isHost && Utils.isLobby)
            {
                GameStartManager.Instance.ResetStartState();
                GameStartManager.Instance.countDownTimer = 0.1f;
                GameStartManager.Instance.BeginGame();
                AmongUsClient.Instance.SendStartGame();
            }
            CheatToggles.forceStartGame = false;
        }

        public static void CompleteMyTasksCheat()
        {
            if (!CheatToggles.completeMyTasks) return;
            foreach (var task in PlayerControl.LocalPlayer.myTasks)
                task.Complete();
            CheatToggles.completeMyTasks = false;
        }

        public static void OpenSabotageMapCheat()
        {
            if (!CheatToggles.sabotageMap) return;
            DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(new MapOptions { Mode = MapOptions.Modes.Sabotage });
            CheatToggles.sabotageMap = false;
        }
    }
}
