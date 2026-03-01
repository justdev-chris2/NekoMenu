using System;
using System.Collections.Generic;
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
        public static bool isHost => AmongUsClient.Instance?.NetMode is NetModes.HostGame or NetModes.LocalGame;
        public static bool isLobby => AmongUsClient.Instance?.GameState == InnerNetClient.GameStates.Joined;
        public static bool isGameStarted => AmongUsClient.Instance?.GameState == InnerNetClient.GameStates.Started;
        public static bool isMeeting => MeetingHud.Instance != null;
        
        public static int GetCurrentMapID()
        {
            if (GameManager.Instance?.LogicOptions == null) return 0;
            return GameManager.Instance.LogicOptions.CurrentGameOptions.MapId;
        }
        
        public static void CompleteTask(PlayerTask task)
        {
            if (task == null || task.IsComplete) return;
            
            task.Complete();
        }
        
        public static void MurderPlayer(PlayerControl target)
        {
            if (target == null || PlayerControl.LocalPlayer == null) return;
            
            PlayerControl.LocalPlayer.MurderPlayer(target, MurderResultFlags.Succeeded);
        }
        
        public static void RpcProtectPlayer(PlayerControl target, int colorId)
        {
            if (PlayerControl.LocalPlayer == null || target == null) return;
            
            PlayerControl.LocalPlayer.ProtectPlayer(target, colorId);
        }
        
        public static void ShowMessage(string message, string title = "NekoMenu")
        {
            if (HudManager.Instance == null) return;
            
            HudManager.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"{title}: {message}");
        }
    }
}
