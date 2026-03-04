using System;
using System.Linq;
using Hazel;
using InnerNet;
using UnityEngine;

namespace NekoMenu
{
    public static class SabotageCheats
    {
        private static bool _reactorSab;
        private static bool _oxygenSab;
        private static bool _commsSab;
        private static bool _elecSab;
        private static bool _unfixableLights;

        public static void HandleReactor(ShipStatus shipStatus, byte mapId)
        {
            switch (mapId)
            {
                case 2: // Polus
                {
                    var labSys = shipStatus.Systems[SystemTypes.Laboratory].TryCast<ReactorSystemType>();
                    if (labSys == null) break;

                    if (CheatToggles.reactorSab != _reactorSab)
                    {
                        shipStatus.RpcUpdateSystem(SystemTypes.Laboratory, CheatToggles.reactorSab ? (byte)16 : (byte)128);
                        _reactorSab = CheatToggles.reactorSab;
                    }
                    CheatToggles.reactorSab = _reactorSab = labSys.IsActive;
                    break;
                }
                case 4: // Airship
                {
                    var heliSys = shipStatus.Systems[SystemTypes.HeliSabotage].TryCast<HeliSabotageSystem>();
                    if (heliSys == null) break;

                    if (CheatToggles.reactorSab != _reactorSab)
                    {
                        if (CheatToggles.reactorSab)
                        {
                            // Lines 212-215 - ALL EXPLICITLY CAST TO BYTE
                            shipStatus.RpcUpdateSystem(SystemTypes.HeliSabotage, (byte)16);
                            shipStatus.RpcUpdateSystem(SystemTypes.HeliSabotage, (byte)17);
                        }
                        else
                        {
                            shipStatus.RpcUpdateSystem(SystemTypes.HeliSabotage, (byte)128);
                        }
                        _reactorSab = CheatToggles.reactorSab;
                    }
                    CheatToggles.reactorSab = _reactorSab = heliSys.IsActive;
                    break;
                }
                default: // Other maps
                {
                    var reactorSys = shipStatus.Systems[SystemTypes.Reactor].TryCast<ReactorSystemType>();
                    if (reactorSys == null) break;

                    if (CheatToggles.reactorSab != _reactorSab)
                    {
                        shipStatus.RpcUpdateSystem(SystemTypes.Reactor, CheatToggles.reactorSab ? (byte)16 : (byte)128);
                        _reactorSab = CheatToggles.reactorSab;
                    }
                    CheatToggles.reactorSab = _reactorSab = reactorSys.IsActive;
                    break;
                }
            }
        }

        public static void HandleOxygen(ShipStatus shipStatus, byte mapId)
        {
            if (mapId == 2 || mapId == 4 || mapId == 5)
            {
                if (CheatToggles.oxygenSab)
                {
                    HudManager.Instance.Notifier.AddDisconnectMessage("Oxygen system not present on this map");
                    CheatToggles.oxygenSab = false;
                }
                return;
            }

            var oxygenSys = shipStatus.Systems[SystemTypes.LifeSupp].TryCast<LifeSuppSystemType>();
            if (oxygenSys == null) return;

            if (CheatToggles.oxygenSab != _oxygenSab)
            {
                shipStatus.RpcUpdateSystem(SystemTypes.LifeSupp, CheatToggles.oxygenSab ? (byte)16 : (byte)128);
                _oxygenSab = CheatToggles.oxygenSab;
            }
            CheatToggles.oxygenSab = _oxygenSab = oxygenSys.IsActive;
        }

        public static void HandleComms(ShipStatus shipStatus, byte mapId)
        {
            if (mapId == 1 || mapId == 5)
            {
                var hqCommsSys = shipStatus.Systems[SystemTypes.Comms].TryCast<HqHudSystemType>();
                if (hqCommsSys == null) return;

                if (CheatToggles.commsSab != _commsSab)
                {
                    if (CheatToggles.commsSab)
                    {
                        shipStatus.RpcUpdateSystem(SystemTypes.Comms, (byte)16);
                        shipStatus.RpcUpdateSystem(SystemTypes.Comms, (byte)17);
                    }
                    else
                    {
                        shipStatus.RpcUpdateSystem(SystemTypes.Comms, (byte)128);
                    }
                    _commsSab = CheatToggles.commsSab;
                }
                CheatToggles.commsSab = _commsSab = hqCommsSys.IsActive;
            }
            else
            {
                var commsSys = shipStatus.Systems[SystemTypes.Comms].TryCast<HudOverrideSystemType>();
                if (commsSys == null) return;

                if (CheatToggles.commsSab != _commsSab)
                {
                    shipStatus.RpcUpdateSystem(SystemTypes.Comms, CheatToggles.commsSab ? (byte)16 : (byte)128);
                    _commsSab = CheatToggles.commsSab;
                }
                CheatToggles.commsSab = _commsSab = commsSys.IsActive;
            }
        }

        public static void HandleElectrical(ShipStatus shipStatus, byte mapId)
        {
            if (mapId == 5)
            {
                if (CheatToggles.elecSab || CheatToggles.unfixableLights)
                {
                    HudManager.Instance.Notifier.AddDisconnectMessage("Electrical system not present on this map");
                    CheatToggles.elecSab = CheatToggles.unfixableLights = false;
                }
                return;
            }

            var elecSys = shipStatus.Systems[SystemTypes.Electrical].TryCast<SwitchSystem>();
            if (elecSys == null) return;

            HandleUnfixLights(shipStatus);

            if (CheatToggles.elecSab != _elecSab)
            {
                if (CheatToggles.elecSab)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        var switchMask = 1 << (i & 0x1F);
                        if ((elecSys.ActualSwitches & switchMask) != (elecSys.ExpectedSwitches & switchMask))
                        {
                            shipStatus.RpcUpdateSystem(SystemTypes.Electrical, (byte)i);
                        }
                    }
                }
                else
                {
                    CheatToggles.unfixableLights = false;
                    byte b = 4;
                    for (var i = 0; i < 5; i++)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                        {
                            b |= (byte)(1 << i);
                        }
                    }
                    shipStatus.RpcUpdateSystem(SystemTypes.Electrical, (byte)(b | 128));
                }
                _elecSab = CheatToggles.elecSab;
            }
            CheatToggles.elecSab = _elecSab = elecSys.IsActive && !_unfixableLights;
        }

        public static void HandleUnfixLights(ShipStatus shipStatus)
        {
            if (CheatToggles.unfixableLights == _unfixableLights) return;

            if (!_unfixableLights)
            {
                CheatToggles.elecSab = false;
            }
            shipStatus.RpcUpdateSystem(SystemTypes.Electrical, (byte)69);
            _unfixableLights = CheatToggles.unfixableLights;
        }

        public static void HandleDoors(ShipStatus shipStatus)
        {
            if (CheatToggles.closeAllDoors)
            {
                DoorsHandler.CloseAllDoors();
                CheatToggles.closeAllDoors = false;
            }
            if (CheatToggles.openAllDoors)
            {
                DoorsHandler.OpenAllDoors();
                CheatToggles.openAllDoors = false;
            }
            if (CheatToggles.spamCloseAllDoors)
            {
                DoorsHandler.CloseAllDoors();
            }
        }

        public static void Process(ShipStatus shipStatus)
        {
            var currentMapID = Utils.GetCurrentMapID();

            HandleReactor(shipStatus, currentMapID);
            HandleOxygen(shipStatus, currentMapID);
            HandleComms(shipStatus, currentMapID);
            HandleElectrical(shipStatus, currentMapID);
            HandleDoors(shipStatus);
        }
    }
}
