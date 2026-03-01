using BepInEx;
using BepInEx.Unity.IL2CPP;
using UnityEngine;

namespace NekoMenu
{
    [BepInPlugin("com.neko.amongusmenu", "NekoMenu", "1.0.0")]
    public class NekoMenuPlugin : BasePlugin
    {
        public override void Load()
        {
            // Just add our existing UI component
            AddComponent<NekoMenuUI>();
            
            Debug.Log("NekoMenu loaded! Press Right Ctrl to open");
        }
    }
}
