using System;
using System.Linq;
using AmongUs.GameOptions;
using Hazel;
using InnerNet;
using UnityEngine;

namespace NekoMenu
{
    public static class Utils
    {
        public static bool isPlayer => PlayerControl.LocalPlayer != null;
        public static bool isHost => AmongUsClient.Instance?.AmHost ?? false;
        public static bool isLobby => AmongUsClient.Instance?.GameState == InnerNetClient.GameStates.Joined;
        public static bool isGameStarted => AmongUsClient.Instance?.GameState == InnerNetClient.GameStates.Started;
        public static bool isMeeting => MeetingHud.Instance != null;
        public static bool isShip => ShipStatus.Instance != null;
        
        public static int GetCurrentMapID()
        {
            if (GameManager.Instance?.LogicOptions == null) return 0;
            return GameOptionsManager.Instance.CurrentGameOptions.MapId;
        }
        
        public static void CompleteTask(PlayerTask task)
        {
            if (task == null || task.IsComplete) return;
            task.Complete();
        }
        
        public static void ShowMessage(string message, string title = "NekoMenu")
        {
            if (HudManager.Instance == null) return;
            HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"{title}: {message}");
        }
    }
}
