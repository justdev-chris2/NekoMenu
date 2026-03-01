using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using Hazel;
using InnerNet;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace NekoMenu
{
    public static class Utils
    {
        public static bool isPlayer => PlayerControl.LocalPlayer != null;
        public static bool isHost => AmongUsClient.Instance?.IsGameHosted ?? false;
        public static bool isLobby => AmongUsClient.Instance?.GameState == InnerNetClient.GameStates.Joined;
        public static bool isGameStarted => AmongUsClient.Instance?.GameState == InnerNetClient.GameStates.Started;
        public static bool isMeeting => MeetingHud.Instance != null;
        
        public static int GetCurrentMapID()
        {
            if (GameManager.Instance?.LogicOptions == null) return 0;
            return (int)GameManager.Instance.LogicOptions.GetByte(ByteOptionNames.MapId);
        }
        
        public static void CompleteTask(PlayerTask task)
        {
            if (task == null || task.IsComplete) return;
            
            task.Complete();
            
            // Send RPC so others see it completed
            if (AmongUsClient.Instance.AmClient)
            {
                task.Id = (uint)new Random().Next();
            }
        }
        
        public static void MurderPlayer(PlayerControl target, MurderResultFlags flag = MurderResultFlags.Succeeded)
        {
            if (target == null || PlayerControl.LocalPlayer == null) return;
            
            // Send murder RPC
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                PlayerControl.LocalPlayer.NetId,
                (byte)RpcCalls.MurderPlayer,
                SendOption.Reliable,
                -1
            );
            
            writer.WriteNetObject(target);
            writer.Write((int)flag);
            
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            
            // Apply locally
            PlayerControl.LocalPlayer.MurderPlayer(target, flag);
        }
        
        public static void ForceSetScanner(PlayerControl player, bool active)
        {
            if (player == null) return;
            
            player.cosmetics.SetScanner(active);
            
            // Send RPC
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                player.NetId,
                (byte)RpcCalls.SetScanner,
                SendOption.Reliable,
                -1
            );
            
            writer.Write(active);
            
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        
        public static void ForcePlayAnimation(byte taskType)
        {
            if (PlayerControl.LocalPlayer == null) return;
            
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                PlayerControl.LocalPlayer.NetId,
                (byte)RpcCalls.StartFakeTask,
                SendOption.Reliable,
                -1
            );
            
            writer.Write(taskType);
            
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        
        public static void RpcProtectPlayer(PlayerControl target, int colorId)
        {
            if (PlayerControl.LocalPlayer == null || target == null) return;
            
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                PlayerControl.LocalPlayer.NetId,
                (byte)RpcCalls.ProtectPlayer,
                SendOption.Reliable,
                -1
            );
            
            writer.WriteNetObject(target);
            writer.Write(colorId);
            
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        
        public static void ShowMessage(string message, string title = "NekoMenu")
        {
            if (HudManager.Instance == null) return;
            
            HudManager.Instance.Notifier.AddDisconnectMessage($"{title}: {message}");
        }
    }
}
