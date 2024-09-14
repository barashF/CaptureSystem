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
using System.Threading;
using SDG.NetTransport;

namespace CaptureSystem.Commands.CallUI
{
    public class ControlUI
    {
        public void HideUI(UnturnedPlayer player)
        {
            for (int i = 0; i < 6; i++)
            {
                EffectManager.sendUIEffectVisibility(1, player.CSteamID, false, "Block_" + (i + 1).ToString(), false);
            }
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);
        }


        public void GetTransport(UnturnedPlayer player, List<TransportInHangar> vehicles, string id_hangar)
        {
            var playerui = Capture.playerUIs.Find(ui => ui.player == player.CSteamID);
            if (playerui == null)
            {
                Capture.playerUIs.Add(new Capture.PlayerUI
                {
                    player = player.CSteamID,
                    vehicles = new List<Transport> { },
                    id_hangar = id_hangar
                }); ;

                playerui = Capture.playerUIs.Find(ui => ui.player == player.CSteamID);
            }
            else
            {
                playerui.vehicles = new List<Transport> { };
                playerui.id_hangar = id_hangar;
            }

            foreach (var i in vehicles)
            {
                var transport = Capture.test.Transport.Find(tr => tr.id == i.id);
                playerui.vehicles.Add(transport);
            }
        }


        public void ShowBlocksUI(UnturnedPlayer player, List<TransportInHangar> vehicles, string id_hangar)
        {
            var hangar = Capture.test.Hangar.Find(h => h.id == id_hangar);
            if (Vector3.Distance(player.Position, hangar.point) > hangar.radius)
            {
                UnturnedChat.Say(player, "Вы находитесь слишком далеко от ангара", UnityEngine.Color.red);
                EffectManager.askEffectClearByID(22224, player.CSteamID);
                player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
                return;
            }
            
            string team = Capture.test.PlayerInf.Find(infa => infa.player == player.CSteamID).team;

            EffectManager.sendUIEffectText(1, player.CSteamID, true, "name_hangar", hangar.name);
            for (int i = 0; i < vehicles.Count; i++)
            {
                ushort id = vehicles[i].id;
                var transport = Capture.test.Transport.Find(vehicle => vehicle.id == id);
                var transportinhangar = Capture.test.TransportInHangar.Find(vehicle => vehicle.id == id & vehicle.team == team);

                EffectManager.sendUIEffectVisibility(1, player.CSteamID, true, "Block_" + (i + 1).ToString(), true);
                EffectManager.sendUIEffectText(1, player.CSteamID, true, "Name_" + (i + 1).ToString(), transport.name);
                EffectManager.sendUIEffectText(1, player.CSteamID, true, "Rang_" + (i + 1).ToString(), "Ранг: " + transport.rang.ToString());
                EffectManager.sendUIEffectText(1, player.CSteamID, true, "Storage_" + (i + 1).ToString(), transportinhangar.existence.ToString() + "/" + transportinhangar.amount.ToString());
                EffectManager.sendUIEffectImageURL(1, player.CSteamID, true, "Image_" + (i + 1).ToString(), Capture.test.TransportImage.Find(image => image.id == id).url_image);
                EffectManager.sendUIEffectVisibility(1, player.CSteamID, !CheckAvailable(player, transport.rang), "unavailable_" + (i + 1).ToString(), !CheckAvailable(player, transport.rang));

            }
            GetTransport(player, vehicles, id_hangar);
        }

        public bool CheckAvailable(UnturnedPlayer player, int rang)
        {
            var infplayer = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            if (rang <= infplayer.rang)
            {
                return true;
            }
            return false;
        }
    }
}
