using Rocket.API;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using Rocket.API.Collections;
using Rocket.Unturned.Chat;
using Logger = Rocket.Core.Logging.Logger;
using System.Timers;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using System;
using SDG.NetTransport;
using System.Threading;
using Steamworks;
using static SDG.Unturned.Provider;

namespace CaptureSystem
{
    public class Capture : RocketPlugin<Config>
    {
        public static Capture Instance;

        public static CaptureSystem test;

        public static Config cfg;

        public static List<PlayerUI> playerUIs = new List<PlayerUI> { };

        public static List<VehicleKD> vehicleKDs = new List<VehicleKD> { };

        public static List<PlayerOnLocation> playerOnLocations = new List<PlayerOnLocation> { };

        public static List<CaptureLocation> captureLocations = new List<CaptureLocation> { };

        public static List<ButtonsFractions> buttonsFractions = new List<ButtonsFractions> { };

        public static List<ButtonsSubclasses> buttonsSubclasses = new List<ButtonsSubclasses> { };

        public static List<KnockedOutPlayer> knockedOutPlayers = new List<KnockedOutPlayer> { };

        public static List<PlayerVehicle> playerVehicles = new List<PlayerVehicle> { };

        public static List<PlayerInf> onServerPlayers = new List<PlayerInf> { };


        public Views.Score score = new Views.Score { };
        public Views.Connected connected = new Views.Connected { };
        public Views.Respawn respawn = new Views.Respawn { };
        public Views.Group groupview = new Views.Group { };
        public Views.Fractions fractionsviews = new Views.Fractions { };
        public Views.Subclasses subclassview = new Views.Subclasses { };
        public Views.Knock knockview = new Views.Knock { };
        public Views.Hangar hangarview = new Views.Hangar { };
        public Views.Capted captview = new Views.Capted { };


        public class PlayerVehicle
        {
            public Steamworks.CSteamID player;

            public ushort vehicle;

            public int time;

            public PlayerVehicle() { }

            public PlayerVehicle(Steamworks.CSteamID player, ushort vehicle, int time)
            {
                this.player = player;
                this.vehicle = vehicle;
                this.time = time;
            }
        }

        public class KnockedOutPlayer
        {
            public Steamworks.CSteamID player;

            public int time;

            public KnockedOutPlayer() { }

            public KnockedOutPlayer(Steamworks.CSteamID player, int time)
            {
                this.player = player;
                this.time = time;
            }
        }

        public class ButtonsSubclasses
        {
            public Steamworks.CSteamID player;

            public List<Subclass> subclasses;

            public ButtonsSubclasses() { }

            public ButtonsSubclasses(Steamworks.CSteamID player, List<Subclass> subclasses)
            {
                this.player = player;
                this.subclasses = subclasses;
            }
        }

        public class ButtonsFractions
        {
            public Steamworks.CSteamID player;

            public List<Fraction> fractions;

            public ButtonsFractions() { }

            public ButtonsFractions(Steamworks.CSteamID player, List<Fraction> fractions)
            {
                this.player = player;
                this.fractions = fractions;
            }
        }

        public class CaptureLocation
        {
            public string id_location;

            public string team;

            public int time;

            public CaptureLocation() { }

            public CaptureLocation(string id_location, string team, int time)
            {
                this.id_location = id_location;
                this.team = team;
                this.time = time;
            }
        }

        public class PlayerOnLocation
        {
            public Steamworks.CSteamID player;

            public string team;

            public string id_location;

            public PlayerOnLocation() { }

            public PlayerOnLocation(Steamworks.CSteamID player, string team, string id_location)
            {
                this.player = player;
                this.team = team;
                this.id_location = id_location;
            }
        }


        public class VehicleKD
        {
            public Steamworks.CSteamID player;

            public int kd;

            public string classification;

            public VehicleKD() { }

            public VehicleKD(Steamworks.CSteamID player, int kd, string classification)
            {
                this.player = player;
                this.kd = kd;
                this.classification = classification;
            }
        }

        public class PlayerUI
        {
            public Steamworks.CSteamID player;

            public List<Transport> vehicles;

            public string id_hangar;
            public PlayerUI() { }

            public PlayerUI(Steamworks.CSteamID player, List<Transport> vehicles, string id_hangar)
            {
                this.player = player;
                this.vehicles = vehicles;
                this.id_hangar = id_hangar;
            }
        }
        

        protected override void Load()
        {
            base.Load();
            Instance = this;
            cfg = Instance.Configuration.Instance;
            EffectManager.onEffectButtonClicked += OnClickted;
            Rocket.Unturned.Events.UnturnedPlayerEvents.OnPlayerUpdatePosition += UnturnedPlayerEvents_OnPlayerUpdatePosition;
            Rocket.Unturned.Events.UnturnedPlayerEvents.OnPlayerDeath += UnturnedPlayerEvents_OnPlayerDeath;
            Rocket.Unturned.Events.UnturnedPlayerEvents.OnPlayerUpdateGesture += UnturnedPlayerEvents_OnPlayerUpdateGesture;
            Rocket.Unturned.Events.UnturnedPlayerEvents.OnPlayerRevive += UnturnedPlayerEvents_OnPlayerRevive;
            UseableConsumeable.onPerformedAid += UseableConsumeable_onPerformedAid;
            DamageTool.damagePlayerRequested += DamageTool_damagePlayerRequested;
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
            U.Events.OnPlayerConnected += onEnemyConnected;
            U.Events.OnPlayerDisconnected += Events_OnPlayerDisconnected;
            
            try
            {
                test = DB.DataBase.GetServerFromDB();
            }
            catch
            {
                DB.DataBase.SerializeServ();
            }
            test = DB.DataBase.GetServerFromDB();
        }


        protected override void Unload()
        {
            base.Unload();
            Instance = null;
        }

        private void onEnemyConnected(UnturnedPlayer player)
        {
            player.Player.stance.onStanceUpdated += () =>
            {
                UnturnedPlayer Uplayer = player;
                var knocked = knockedOutPlayers.Find(k => k.player == player.CSteamID);
                if (knocked != null)
                {
                    player.Player.stance.stance = EPlayerStance.PRONE;
                }
            };
            player.Player.equipment.onEquipRequested += (PlayerEquipment equipment, ItemJar jar, ItemAsset asset, ref bool shouldAllow) =>
            {
                UnturnedPlayer Uplayer = player;
                var knocked = knockedOutPlayers.Find(k => k.player == player.CSteamID);
                if (knocked != null)
                {
                    shouldAllow = false;
                }
            };
        }


        private void DamageTool_damagePlayerRequested(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            knockview.TryKnockPLayer(parameters);
        }

        private void UseableConsumeable_onPerformedAid(Player instigator, Player target)
        {
            UnturnedPlayer medic = UnturnedPlayer.FromPlayer(instigator);
            UnturnedPlayer knocked = UnturnedPlayer.FromPlayer(target);

            var knockedPlayer = knockedOutPlayers.Find(pl => pl.player == knocked.CSteamID);
            if(knockedPlayer == null)
            {
                return;
            }

            knockedOutPlayers.RemoveAll(pl => pl.player == knocked.CSteamID);
            score.UpdateScore(medic, 2);
        }

        private void Events_OnPlayerDisconnected(UnturnedPlayer player)
        {
            ExitLocation(player);
            knockedOutPlayers.RemoveAll(pl => pl.player == player.CSteamID);
            onServerPlayers.RemoveAll(pl => pl.player == player.CSteamID);
        }


        public ushort FindVehicleTeam(string team, List<ushort> vehicles)
        {
            if (test.TransportInHangar.Find(transport => transport.id == vehicles[0] & transport.team == team) != null)
            {
                return vehicles[0];
            }
            return vehicles[1];
        }

        public void MinusBoost(List<Boost> boosts, string team)
        {
            var check_team = test.Team.Find(t => t.id == team);
            if(check_team == null)
            {
                return;
            }

            foreach (var boost in boosts)
            {
                ushort vehicle = FindVehicleTeam(team, new List<ushort> { boost.teamA, boost.teamB });
                var vehicleinhangar = test.TransportInHangar.Find(tr => tr.id == vehicle & tr.team == team);

                vehicleinhangar.amount -= boost.amount_storage;
                vehicleinhangar.supply -= boost.supply;

                DB.DataBase.Save(test);
            }
        }
        public void PlusBoost(List<Boost> boosts, string team)
        {
            foreach (var boost in boosts)
            {
                ushort vehicle = FindVehicleTeam(team, new List<ushort> { boost.teamA, boost.teamB });
                var vehicleinhangar = test.TransportInHangar.Find(tr => tr.id == vehicle & tr.team == team);

                vehicleinhangar.amount += boost.amount_storage;
                vehicleinhangar.supply += boost.supply;

                DB.DataBase.Save(test);
            }
        }

        float Last_time = 0;
        float Last_hour = 0;
        private void Update()
        {
            float time = Time.time;

            if ((time - Last_time) > 0.99)
            {
                hangarview.UpdateKD();
                for (int i = vehicleKDs.Count - 1; i >= 0; i--)
                {
                    vehicleKDs[i].kd -= 1;
                    if (vehicleKDs[i].kd < 1)
                    {
                        vehicleKDs.RemoveAt(i);
                    }
                }
                Last_time = time;
                if (captureLocations.Count != 0)
                {
                    captureLocations[0].time -= 1;
                    captview.UpdateTimerCapt(captureLocations[0]);

                    if (captureLocations[0].time < 1)
                    {
                        var capt = captureLocations[0];
                        var location = test.Location.Find(loc => loc.id == capt.id_location);

                        score.ScoreForCapt(capt.team, location.id);
                        MinusBoost(location.boosts, location.team);
                        PlusBoost(location.boosts, capt.team);

                        location.team = capt.team;
                        DB.DataBase.Save(test);
                        UnturnedChat.Say($"{capt.team} успешно захватывает: {location.name}", UnityEngine.Color.green);

                        captureLocations.RemoveAll(c => c.id_location == capt.id_location);
                        UpdateUIcapture();
                    }
                }
                
                knockview.TryKillKnocked();
            }
            if ((time - Last_hour) > 3599)
            {
                foreach (var i in test.TransportInHangar)
                {
                    if ((i.amount - i.existence) <= i.supply)
                    {
                        i.existence += (i.amount - i.existence);
                    }
                    else
                    {
                        i.existence += i.supply;
                    }
                    DB.DataBase.Save(test);
                }
                Last_hour = time;
            }
        }

        public static Location GetLocation(Vector3 position)
        {
            foreach (var i in test.Location)
            {
                if (Vector3.Distance(i.point, position) <= i.radius)
                {
                    return i;
                }
            }
            return null;
        }

        public void TryDeleteCapture(UnturnedPlayer player, string id_location)
        {
            var capt = captureLocations.Find(loc => loc.id_location == id_location);
            if (capt == null)
            {
                return;
            }

            var players_on_location = playerOnLocations.FindAll(playerloc => playerloc.id_location == id_location & playerloc.team == capt.team);
            if (players_on_location.Count < cfg.min_amount_capture)
            {
                UnturnedChat.Say("Захват локации " + test.Location.Find(l => l.id == capt.id_location).name + " провалился", UnityEngine.Color.red);
                DestroyForPlayersCapture(id_location);
                captureLocations.RemoveAll(c => c.id_location == capt.id_location);
            }
        }

        public void DestroyUILocation(UnturnedPlayer player)
        {
            EffectManager.askEffectClearByID(22225, player.CSteamID);
        }
        public void DestroyUICapture(UnturnedPlayer player)
        {
            EffectManager.askEffectClearByID(22226, player.CSteamID);
        }

        public void UpdateUIcapture()
        {
            foreach (var i in playerOnLocations)
            {
                EffectManager.askEffectClearByID(22225, i.player);
            }
            playerOnLocations.Clear();
        }
        public void GetUILocation(UnturnedPlayer player, string id_location)
        {
            Location location = test.Location.Find(loc => loc.id == id_location);
            EffectManager.sendUIEffect(22225, 3, player.CSteamID, true);
            EffectManager.sendUIEffectText(3, player.CSteamID, true, "Name", location.name);
            if (location.team == "")
            {
                EffectManager.sendUIEffectText(3, player.CSteamID, true, "Owner", "Локация не захвачена");
            }
            else
            {
                EffectManager.sendUIEffectText(3, player.CSteamID, true, "Owner", "Локацию удерживает " + test.Team.Find(team => team.id == location.team).name);
            }
        }
        public void DestroyForPlayersCapture(string id_location)
        {
            var playersloc = playerOnLocations.FindAll(location => location.id_location == id_location);
            foreach (var i in playersloc)
            {
                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(i.player);
                DestroyUICapture(player);
            }
        }

        public void ExitLocation(UnturnedPlayer player)
        {
            var playerloc = playerOnLocations.Find(loc => loc.player == player.CSteamID);
            if (playerloc == null)
            {
                return;
            }

            playerOnLocations.RemoveAll(playerlc => playerlc.player == playerloc.player);
            TryDeleteCapture(player, playerloc.id_location);
            DestroyUILocation(player);
            DestroyUICapture(player);
        }

        public void EnterLocation(UnturnedPlayer player, string id_location)
        {
            var playerloc = playerOnLocations.Find(pl => pl.player == player.CSteamID & pl.id_location == id_location);
            if (playerloc != null)
            {
                return;
            }
            string team = test.PlayerInf.Find(inf => inf.player == player.CSteamID).team;
            playerOnLocations.Add(new PlayerOnLocation
            {
                player = player.CSteamID,
                id_location = id_location,
                team = team
            });
            GetUILocation(player, id_location);
        }


        private void UnturnedPlayerEvents_OnPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            ExitLocation(player);
            score.PlucScoreKill(UnturnedPlayer.FromCSteamID(murderer), player);
            knockedOutPlayers.RemoveAll(pl => pl.player == player.CSteamID);
        }

        private void UnturnedPlayerEvents_OnPlayerUpdatePosition(UnturnedPlayer player, Vector3 position)
        {
            Location location = GetLocation(position);
            if (location == null)
            {
                ExitLocation(player);
                return;
            }
            EnterLocation(player, location.id);
        }


        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            var check = test.Team[0];
            if(check != null)
            {
                connected.TryTellUIStart(player.CSteamID);
                connected.TryInviteInGroup(player.CSteamID);
                score.SendRankUI(player);
                connected.AddOnlinePlayer(player);
            }
            
        }

        private void UnturnedPlayerEvents_OnPlayerUpdateGesture(UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            
        }

        private void UnturnedPlayerEvents_OnPlayerRevive(UnturnedPlayer player, Vector3 position, byte angle)
        {
            respawn.OnRespawn(player);
        }

        public bool CheckRang(UnturnedPlayer player, Transport vehicle)
        {
            var playerinf = test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            if (playerinf.rang >= vehicle.rang)
            {
                return true;
            }
            return false;
        }


        public void SpawnTransport(UnturnedPlayer player, Transport vehicle, string id_hangar)
        {
            var points = test.Hangar.Find(hangar => hangar.id == id_hangar).pointSpawnTransports;
            var point = points.Find(p => p.classification == vehicle.classification);

            VehicleManager.spawnLockedVehicleForPlayerV2(vehicle.id, point.point, point.quaternion, player.Player);
            vehicle.GetItems(player);

        }


        private void OnClickted(Player player, string buttonName)
        {
            UnturnedPlayer untplayer = UnturnedPlayer.FromPlayer(player);
            var playerui = playerUIs.Find(ui => ui.player == untplayer.CSteamID);

            if (buttonName == "Close_hangar")
            {
                EffectManager.askEffectClearByID(22224, untplayer.CSteamID);
                player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
                return;
            }
            else if (buttonName == "close_group")
            {
                EffectManager.askEffectClearByID(22229, untplayer.CSteamID);
                player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
                return;
            }
            else if (buttonName == "close_subclass")
            {
                EffectManager.askEffectClearByID(22230, untplayer.CSteamID);
                player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
                return;
            }
            else if (buttonName.Contains("Block_"))
            {
                for (int i = 0; i < 6; i++)
                {
                    string button = "Block_" + (i + 1).ToString();
                    if (button == buttonName)
                    {
                        var kd = vehicleKDs.Find(veh => veh.player == untplayer.CSteamID & veh.classification == playerui.vehicles[i].classification);
                        var hangar = test.Hangar.Find(han => han.id == playerui.id_hangar);
                        var vehicle_hangar = hangar.transportInHangars.Find(veh => veh.id == playerui.vehicles[i].id);

                        var transportinhangar = test.TransportInHangar.Find(transport => transport.id == vehicle_hangar.id & transport.team == vehicle_hangar.team);

                        if (!CheckRang(untplayer, playerui.vehicles[i]))
                        {
                            UnturnedChat.Say(untplayer, "Ваш ранг слишком низий", UnityEngine.Color.red);
                        }
                        else if (transportinhangar.existence == 0)
                        {
                            UnturnedChat.Say(untplayer, "В ангаре закончилась техника, ждите поставки", UnityEngine.Color.red);
                        }
                        else if (kd != null)
                        {
                            UnturnedChat.Say(untplayer, $"Данный тип транспорта будет доступен через: {kd.kd} seconds", UnityEngine.Color.red);
                        }
                        else
                        {
                            SpawnTransport(untplayer, playerui.vehicles[i], playerui.id_hangar);
                            transportinhangar.existence -= 1;
                            DB.DataBase.Save(test);

                            vehicleKDs.Add(new VehicleKD
                            {
                                player = untplayer.CSteamID,
                                kd = playerui.vehicles[i].kd,
                                classification = playerui.vehicles[i].classification
                            });

                            hangarview.AddPlayerVehicle(untplayer, playerui.vehicles[i].id);
                        }

                        EffectManager.askEffectClearByID(22224, untplayer.CSteamID);
                        player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
                    }
                }
            }
            else if (buttonName.Contains("choice"))
            {
                string team_id = buttonName.Replace("choice_", "");
                groupview.ChangeTeam(untplayer, team_id);
                EffectManager.askEffectClearByID(22227, untplayer.CSteamID);
                player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
                untplayer.Kick("Необходимо перезайти после выбора команды");
            }
            else if (buttonName.Contains("group_"))
            {
                int id = int.Parse(buttonName.Replace("group_", "")) - 1;
                Fraction fraction = buttonsFractions.Find(b => b.player == untplayer.CSteamID).fractions[id];
                fractionsviews.ChangeFraction(untplayer, fraction.id);
            }
            else if (buttonName.Contains("subclass_"))
            {
                int id = int.Parse(buttonName.Replace("subclass_", "")) - 1;
                Subclass subclass = buttonsSubclasses.Find(s => s.player == untplayer.CSteamID).subclasses[id];
                subclassview.ChangeSubclass(untplayer, subclass.id);
            }
        }
    }
}
