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

namespace CaptureSystem.Commands.Transport_command
{
    class GetAmmunition : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "ammu" };

        public List<string> Permissions => new List<string> { "ammunition" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            var playerVehicle = Capture.playerVehicles.Find(plvh => plvh.player == player.CSteamID);
            var playerInf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            var team = Capture.test.Team.Find(tm => tm.id == playerInf.team);

            if(Vector3.Distance(team.point, player.Position) > 400)
            {
                UnturnedChat.Say(player, "Вы слишком далеко от базы", UnityEngine.Color.red);
                return;
            }

            if(playerVehicle.time > 0)
            {
                UnturnedChat.Say(player, $"Дополнительный боекомплект будет доступен через: {playerVehicle.time}", UnityEngine.Color.red);
                return;
            }

            var vehicle = Capture.test.Transport.Find(tr => tr.id == playerVehicle.vehicle);
            vehicle.GetItems(player);
            playerVehicle.time = 1200;
            UnturnedChat.Say(player, "Боекомплект получен");
        }
    }
}
