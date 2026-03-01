using BepInEx;
using BepInEx.Unity.IL2CPP;
using UnityEngine;

namespace NekoMenu
{
    [BepInPlugin("com.justdevchris.nekomenu", "NekoMenu", "1.0.1")]
    public class NekoMenuPlugin : BasePlugin
    {
        public override void Load()
        {
            AddComponent<NekoMenuUI>();
            Debug.Log("NekoMenu loaded! Press Right Ctrl to open");
        }
    }
}
