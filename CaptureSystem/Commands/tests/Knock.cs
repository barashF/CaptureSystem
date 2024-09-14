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
using Rocket.Unturned.Permissions;
using Rocket.API.Extensions;
using Rocket.API.Serialisation;


namespace CaptureSystem.Commands.tests
{
    public class Knock : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "knock" };

        public List<string> Permissions => new List<string> { "admin" };

        public Views.Connected connected = new Views.Connected { };


        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = UnturnedPlayer.FromName(command[0]);

            player.Player.stance.stance = EPlayerStance.PRONE;
            Capture.knockedOutPlayers.Add(new Capture.KnockedOutPlayer
            {
                player = player.CSteamID,
                time = 10
            });
        }
    }
}
