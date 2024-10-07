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

namespace CapturSystem.Commands.Location_command
{
    class Capture : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "capture" };

        public List<string> Permissions => new List<string> { "capture" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            var playerinf = CaptureSystem.Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            var location = CaptureSystem.Capture.GetLocation(player.Position);
            
            if (location == null)
            {
                UnturnedChat.Say(player, "Вы должны находиться на локации!", UnityEngine.Color.red);
                return;
            }
            
            if (location.team == playerinf.team)
            {
                UnturnedChat.Say(player, "Локация уже захвачена!", UnityEngine.Color.red);
                return;
            }
            UnturnedChat.Say(player, "3", UnityEngine.Color.red);
            if ((playerinf.rang + 1) < CaptureSystem.Capture.cfg.min_rang)
            {
                UnturnedChat.Say(player, "Вы не можете начать захват, ваш ранг слишком низкий!", UnityEngine.Color.red);
                return;
            }
            UnturnedChat.Say(player, "4", UnityEngine.Color.red);
            if (CaptureSystem.Capture.captureLocations.Count != 0)
            {
                UnturnedChat.Say(player, "Идёт другой захват!", UnityEngine.Color.red);
                return;
            }
            UnturnedChat.Say(player, "5", UnityEngine.Color.red);
            if (CaptureSystem.Capture.playerOnLocations.FindAll(pl => pl.id_location == location.id & pl.team == playerinf.team).Count < CaptureSystem.Capture.cfg.min_amount_capture)
            {
                UnturnedChat.Say(player, $"На локации должно находиться хотя бы {CaptureSystem.Capture.cfg.min_amount_capture} членов вашей команды" , UnityEngine.Color.red);
                return;
            }
            UnturnedChat.Say(player, "6", UnityEngine.Color.red);
            if (CountEnemy(playerinf.team) < CaptureSystem.Capture.cfg.min_amount_capture)
            {
                UnturnedChat.Say(player, "Числинность команды соперника слишком мала", UnityEngine.Color.red);
                return;
            }
            UnturnedChat.Say(player, "7", UnityEngine.Color.red);
            CaptureSystem.Capture.captureLocations.Add(new CaptureSystem.Capture.CaptureLocation
            {
                id_location = location.id,
                team = playerinf.team,
                time = 900
            });
            
            UnturnedChat.Say($"{playerinf.team} начинает захват: {location.name}", UnityEngine.Color.yellow);
        }

        public int CountEnemy(string id_team)
        {
            var enemy = CaptureSystem.Capture.test.Team.Find(team => team.id != id_team).id;
            var count_enemy = CaptureSystem.Capture.onServerPlayers.FindAll(en => en.team == enemy).Count;
            return count_enemy;
        }
    }
}
