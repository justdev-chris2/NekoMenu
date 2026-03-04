using System.Collections.Generic;
using System.Linq;
using InnerNet;
using UnityEngine;

namespace NekoMenu
{
    public static class DoorsHandler
    {
        public static List<SystemTypes> GetRoomsWithDoors()
        {
            if (ShipStatus.Instance == null || ShipStatus.Instance.AllDoors.Count <= 0) return new List<SystemTypes>();
            return ShipStatus.Instance.AllDoors.Select(d => d.Room).Distinct().ToList();
        }

        public static List<OpenableDoor> GetDoorsInRoom(SystemTypes room)
        {
            if (ShipStatus.Instance == null || ShipStatus.Instance.AllDoors.Count <= 0) return new List<OpenableDoor>();
            return ShipStatus.Instance.AllDoors.Where(d => d.Room == room).ToList();
        }

        public static void OpenAllDoors()
        {
            if (ShipStatus.Instance == null) return;
            foreach (var door in ShipStatus.Instance.AllDoors)
            {
                if (door != null)
                    door.SetDoorwayOpen(true);
            }
        }

        public static void CloseAllDoors()
        {
            if (ShipStatus.Instance == null) return;
            foreach (var door in ShipStatus.Instance.AllDoors)
            {
                if (door != null)
                    door.SetDoorwayOpen(false);
            }
        }
    }
}
