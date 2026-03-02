using System;
using System.Linq;
using Hazel;
using InnerNet;
using UnityEngine;

namespace NekoMenu
{
    public static class ChaosCheats
    {
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
            
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player != null && player != PlayerControl.LocalPlayer && player.MyPhysics != null)
                {
                    // Stop their movement completely
                    player.MyPhysics.body.velocity = Vector2.zero;
                    player.MyPhysics.body.constraints = RigidbodyConstraints2D.FreezeAll;
                    
                    // Keep sending freeze RPC to maintain it
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                        player.NetId,
                        (byte)RpcCalls.SetColor,
                        SendOption.Reliable,
                        -1
                    );
                    writer.Write(player.cosmetics.ColorId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
            }
            
            CheatToggles.freezeAll = false;
        }

        public static void UnfreezeAllCheat()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player != null && player.MyPhysics != null)
                {
                    // Restore movement
                    player.MyPhysics.body.constraints = RigidbodyConstraints2D.None;
                }
            }
        }
    }
}
