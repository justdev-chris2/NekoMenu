using AmongUs.GameOptions;
using UnityEngine;

namespace NekoMenu
{
    public static class RoleCheats
    {
        public static void HandleEngineerCheats(EngineerRole engineerRole)
        {
            if (CheatToggles.endlessVentTime)
                engineerRole.inVentTimeRemaining = float.MaxValue;
            else if (engineerRole.inVentTimeRemaining > engineerRole.GetCooldown())
                engineerRole.inVentTimeRemaining = engineerRole.GetCooldown();

            if (CheatToggles.noVentCooldown && engineerRole.cooldownSecondsRemaining > 0f)
            {
                engineerRole.cooldownSecondsRemaining = 0f;
                DestroyableSingleton<HudManager>.Instance.AbilityButton.ResetCoolDown();
                DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCooldownFill(0f);
            }
        }

        public static void HandleShapeshifterCheats(ShapeshifterRole shapeshifterRole)
        {
            if (CheatToggles.endlessSsDuration)
                shapeshifterRole.durationSecondsRemaining = float.MaxValue;
            else if (shapeshifterRole.durationSecondsRemaining > GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterDuration))
                shapeshifterRole.durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterDuration);
        }

        public static void HandleScientistCheats(ScientistRole scientistRole)
        {
            if (CheatToggles.noVitalsCooldown)
                scientistRole.currentCooldown = 0f;

            if (CheatToggles.endlessBattery)
                scientistRole.currentCharge = float.MaxValue;
            else if (scientistRole.currentCharge > scientistRole.RoleCooldownValue)
                scientistRole.currentCharge = scientistRole.RoleCooldownValue;
        }

        public static void HandleTrackerCheats(TrackerRole trackerRole)
        {
            if (CheatToggles.noTrackingCooldown)
            {
                trackerRole.cooldownSecondsRemaining = 0f;
                trackerRole.delaySecondsRemaining = 0f;
                DestroyableSingleton<HudManager>.Instance.AbilityButton.ResetCoolDown();
                DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCooldownFill(0f);
            }

            if (CheatToggles.noTrackingDelay && MapBehaviour.Instance != null)
                MapBehaviour.Instance.trackedPointDelayTime = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDelay);

            if (CheatToggles.endlessTracking)
                trackerRole.durationSecondsRemaining = float.MaxValue;
            else if (trackerRole.durationSecondsRemaining > GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDuration))
                trackerRole.durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDuration);
        }

        public static void UseVentCheat(HudManager hudManager)
        {
            try
            {
                if (!PlayerControl.LocalPlayer.Data.Role.CanVent && !PlayerControl.LocalPlayer.Data.IsDead)
                    hudManager.ImpostorVentButton.gameObject.SetActive(CheatToggles.useVents);
            } catch { }
        }

        public static void WalkInVentCheat()
        {
            try
            {
                if (!CheatToggles.walkVent) return;
                PlayerControl.LocalPlayer.inVent = false;
                PlayerControl.LocalPlayer.moveable = true;
            } catch { }
        }

        public static void KickVentsCheat()
        {
            if (!CheatToggles.kickVents) return;

            foreach(var vent in ShipStatus.Instance.AllVents)
                VentilationSystem.Update(VentilationSystem.Operation.BootImpostors, vent.Id);

            CheatToggles.kickVents = false;
        }

        public static void ProtectCheat()
        {
            if (!Utils.isHost || Utils.isLobby) return;

            foreach (var player in CheatToggles.playersToProtect)
            {
                if (player != null && player.protectedByGuardianId == -1)
                    PlayerControl.LocalPlayer.ProtectPlayer(player, PlayerControl.LocalPlayer.cosmetics.ColorId);
            }
        }
    }
}
