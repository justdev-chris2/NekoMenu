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
        
        public static int GetCurrentMapID()
        {
            if (GameManager.Instance?.LogicOptions == null) return 0;
            return GameOptionsManager.Instance.CurrentGameOptions.MapId;
        }
        
        public static void ShowMessage(string message, string title = "NekoMenu")
        {
            if (HudManager.Instance == null) return;
            HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"{title}: {message}");
        }
        
        public static void SendNotification(string message, float duration = 3f)
        {
            if (PlayerControl.LocalPlayer == null || AmongUsClient.Instance == null) return;
            
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                PlayerControl.LocalPlayer.NetId,
                (byte)RpcCalls.SetNotifier,
                SendOption.Reliable,
                -1
            );
            writer.Write(message);
            writer.Write(duration);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            
            if (HudManager.Instance != null)
                HudManager.Instance.Notifier.AddItem(message);
        }
    }
}
