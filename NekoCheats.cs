using System;
using System.Linq;
using AmongUs.GameOptions;
using Hazel;
using InnerNet;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace NekoMenu
{
    public static class NekoCheats
    {
        public static void CloseMeetingCheat()
        {
            if (!CheatToggles.closeMeeting) return;

            if (Utils.isMeeting)
            {
                MeetingHud.Instance.DespawnOnDestroy = false;
                UnityEngine.Object.Destroy(MeetingHud.Instance.gameObject);

                DestroyableSingleton<HudManager>.Instance.StartCoroutine(DestroyableSingleton<HudManager>.Instance.CoFadeFullScreen(Color.black, Color.clear, 0.2f, false));
                PlayerControl.LocalPlayer.SetKillTimer(GameManager.Instance.LogicOptions.GetKillCooldown());
                ShipStatus.Instance.EmergencyCooldown = GameManager.Instance.LogicOptions.GetEmergencyCooldown();
                Camera.main.GetComponent<FollowerCamera>().Locked = false;
                DestroyableSingleton<HudManager>.Instance.SetMapButtonEnabled(true);
                DestroyableSingleton<HudManager>.Instance.SetHudActive(true);
                ControllerManager.Instance.CloseAndResetAll();
            }
            else if (ExileController.Instance)
            {
                ExileController.Instance.ReEnableGameplay();
                ExileController.Instance.WrapUp();
            }

            CheatToggles.closeMeeting = false;
        }

        public static void SkipMeetingCheat()
        {
            if (!CheatToggles.skipMeeting) return;

            if (Utils.isMeeting)
            {
                MeetingHud.Instance.RpcVotingComplete(new Il2CppStructArray<MeetingHud.VoterState>(0L), null, true);
            }

            CheatToggles.skipMeeting = false;
        }

        public static void CallMeetingCheat()
        {
            if (!CheatToggles.callMeeting) return;

            if (Utils.isHost)
            {
                MeetingRoomManager.Instance.AssignSelf(PlayerControl.LocalPlayer, null);
                DestroyableSingleton<HudManager>.Instance.OpenMeetingRoom(PlayerControl.LocalPlayer);
                PlayerControl.LocalPlayer.RpcStartMeeting(null);
            }
            else
            {
                PlayerControl.LocalPlayer.CmdReportDeadBody(null);
            }

            CheatToggles.callMeeting = false;
        }

        public static void ForceStartGameCheat()
{
    if (!CheatToggles.forceStartGame) return;

    if (Utils.isHost && Utils.isLobby)
    {
        // Force game start
        GameStartManager.Instance.ResetStartState();
        GameStartManager.Instance.countDownTimer = 0.1f;
        GameStartManager.Instance.StartGame();
        AmongUsClient.Instance.SendStartGame();
    }

    CheatToggles.forceStartGame = false;
}

        public static void NoKillCdCheat(PlayerControl playerControl)
        {
            if (CheatToggles.zeroKillCd && playerControl.killTimer > 0f)
            {
                playerControl.SetKillTimer(0f);
            }
        }

        public static void CompleteMyTasksCheat()
        {
            if (CheatToggles.completeMyTasks)
            {
                foreach (var task in PlayerControl.LocalPlayer.myTasks)
                {
                    task.Complete();
                }

                CheatToggles.completeMyTasks = false;
            }
        }

        public static void OpenSabotageMapCheat()
        {
            if (!CheatToggles.sabotageMap) return;

            DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(new MapOptions
            {
                Mode = MapOptions.Modes.Sabotage
            });

            CheatToggles.sabotageMap = false;
        }

        public static void HandleEngineerCheats(EngineerRole engineerRole)
        {
            if (CheatToggles.endlessVentTime)
            {
                engineerRole.inVentTimeRemaining = float.MaxValue;
            }
            else if (engineerRole.inVentTimeRemaining > engineerRole.GetCooldown())
            {
                engineerRole.inVentTimeRemaining = engineerRole.GetCooldown();
            }

            if (CheatToggles.noVentCooldown)
            {
                if (engineerRole.cooldownSecondsRemaining > 0f)
                {
                    engineerRole.cooldownSecondsRemaining = 0f;
                    DestroyableSingleton<HudManager>.Instance.AbilityButton.ResetCoolDown();
                    DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCooldownFill(0f);
                }
            }
        }

        public static void HandleShapeshifterCheats(ShapeshifterRole shapeshifterRole)
        {
            if (CheatToggles.endlessSsDuration)
            {
                shapeshifterRole.durationSecondsRemaining = float.MaxValue;
            }
            else if (shapeshifterRole.durationSecondsRemaining > GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterDuration))
            {
                shapeshifterRole.durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.ShapeshifterDuration);
            }
        }

        public static void HandleScientistCheats(ScientistRole scientistRole)
        {
            if (CheatToggles.noVitalsCooldown)
            {
                scientistRole.currentCooldown = 0f;
            }

            if (CheatToggles.endlessBattery)
            {
                scientistRole.currentCharge = float.MaxValue;
            }
            else if (scientistRole.currentCharge > scientistRole.RoleCooldownValue)
            {
                scientistRole.currentCharge = scientistRole.RoleCooldownValue;
            }
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
            {
                MapBehaviour.Instance.trackedPointDelayTime = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDelay);
            }

            if (CheatToggles.endlessTracking)
            {
                trackerRole.durationSecondsRemaining = float.MaxValue;
            }
            else if (trackerRole.durationSecondsRemaining > GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDuration))
            {
                trackerRole.durationSecondsRemaining = GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.TrackerDuration);
            }
        }

        public static void UseVentCheat(HudManager hudManager)
        {
            try
            {
                if (!PlayerControl.LocalPlayer.Data.Role.CanVent && !PlayerControl.LocalPlayer.Data.IsDead)
                {
                    hudManager.ImpostorVentButton.gameObject.SetActive(CheatToggles.useVents);
                }
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
            {
                VentilationSystem.Update(VentilationSystem.Operation.BootImpostors, vent.Id);
            }

            CheatToggles.kickVents = false;
        }

        public static void KillAllCheat()
        {
            if (!CheatToggles.killAll) return;

            if (Utils.isLobby)
            {
                HudManager.Instance.Notifier.AddDisconnectMessage("Killing in lobby disabled");
            }
            else
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead)
                    {
                        PlayerControl.LocalPlayer.MurderPlayer(player, MurderResultFlags.Succeeded);
                    }
                }
            }

            CheatToggles.killAll = false;
        }

        public static void KillAllCrewCheat()
        {
            if (!CheatToggles.killAllCrew) return;

            if (Utils.isLobby)
            {
                HudManager.Instance.Notifier.AddDisconnectMessage("Killing in lobby disabled");
            }
            else
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead && player.Data.Role.TeamType == RoleTeamTypes.Crewmate)
                    {
                        PlayerControl.LocalPlayer.MurderPlayer(player, MurderResultFlags.Succeeded);
                    }
                }
            }

            CheatToggles.killAllCrew = false;
        }

        public static void KillAllImpsCheat()
        {
            if (!CheatToggles.killAllImps) return;

            if (Utils.isLobby)
            {
                HudManager.Instance.Notifier.AddDisconnectMessage("Killing in lobby disabled");
            }
            else
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead && player.Data.Role.TeamType == RoleTeamTypes.Impostor)
                    {
                        PlayerControl.LocalPlayer.MurderPlayer(player, MurderResultFlags.Succeeded);
                    }
                }
            }

            CheatToggles.killAllImps = false;
        }

        public static void KillSelectedCheat()
        {
            if (!CheatToggles.killSelected || CheatToggles.selectedTargetId < 0) return;
            
            var target = PlayerControl.AllPlayerControls.ToArray()
                .FirstOrDefault(p => p != null && p.PlayerId == CheatToggles.selectedTargetId);
            
            if (target != null)
            {
                // Force kill via RPC
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                    PlayerControl.LocalPlayer.NetId,
                    (byte)RpcCalls.MurderPlayer,
                    SendOption.Reliable,
                    -1
                );
                writer.WriteNetObject(target);
                writer.Write((int)MurderResultFlags.Succeeded);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                
                PlayerControl.LocalPlayer.MurderPlayer(target, MurderResultFlags.Succeeded);
                Utils.ShowMessage($"Killed {target.Data.PlayerName}");
            }
            
            CheatToggles.killSelected = false;
        }

        public static void KillSelfCheat()
        {
            if (!CheatToggles.killSelf || PlayerControl.LocalPlayer == null) return;
            
            PlayerControl.LocalPlayer.MurderPlayer(PlayerControl.LocalPlayer, MurderResultFlags.Succeeded);
            CheatToggles.killSelf = false;
        }

        public static void ProtectCheat()
        {
            if (!Utils.isHost || Utils.isLobby) return;

            foreach (var player in CheatToggles.playersToProtect)
            {
                if (player != null && player.protectedByGuardianId == -1)
                {
                    PlayerControl.LocalPlayer.ProtectPlayer(player, PlayerControl.LocalPlayer.cosmetics.ColorId);
                }
            }
        }

        public static void TeleportCursorCheat()
        {
            if (!CheatToggles.teleportCursor) return;

            if (Input.GetMouseButtonDown(1))
            {
                PlayerControl.LocalPlayer.NetTransform.RpcSnapTo(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }

        public static void NoClipCheat()
        {
            try
            {
                PlayerControl.LocalPlayer.Collider.enabled = !(CheatToggles.noClip || PlayerControl.LocalPlayer.onLadder);
            } catch { }
        }

        public static void ReviveCheat()
        {
            if (!CheatToggles.fakeRevive) return;

            PlayerControl.LocalPlayer.Revive();
            CheatToggles.fakeRevive = false;
        }

        public static void FakeRoleCheat()
        {
            if (!CheatToggles.fakeRole || PlayerControl.LocalPlayer == null) return;
            
            // This would need proper role changing logic
            // For now just toggle off
            CheatToggles.fakeRole = false;
        }
    }
}
