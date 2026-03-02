using System;
using System.Linq;
using Hazel;
using InnerNet;
using UnityEngine;

namespace NekoMenu
{
    public static class ChaosCheats
    {
        private static bool isFrozen = false;
        private static Vector3[] frozenPositions;

        public static void FreezeEveryoneCheat()
        {
            if (!CheatToggles.freezeEveryone || PlayerControl.LocalPlayer == null) return;
            
            var players = PlayerControl.AllPlayerControls.ToArray();
            
            if (!isFrozen)
            {
                // Freeze them
                frozenPositions = new Vector3[players.Length];
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] != null)
                    {
                        frozenPositions[i] = players[i].transform.position;
                    }
                }
                isFrozen = true;
            }
            else
            {
                // Keep snapping them back to frozen positions
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] != null && frozenPositions[i] != null)
                    {
                        players[i].NetTransform.RpcSnapTo(frozenPositions[i]);
                    }
                }
            }
            
            CheatToggles.freezeEveryone = false;
        }

        public static void UnfreezeEveryoneCheat()
        {
            if (!CheatToggles.unfreezeEveryone) return;
            
            isFrozen = false;
            frozenPositions = null;
            
            CheatToggles.unfreezeEveryone = false;
        }
    }
}
