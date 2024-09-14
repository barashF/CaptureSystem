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
using Rocket.Core.Permissions;
using Rocket.Core;

namespace CaptureSystem.Commands.tests
{
    class groups : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "ch" };

        public List<string> Permissions => new List<string> { "admin" };

        public Views.Connected connected = new Views.Connected { };


        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            var playerinf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            if(playerinf != null)
            {
                Capture.test.PlayerInf.RemoveAll(inf => inf.player == player.CSteamID);
                DB.DataBase.Save(Capture.test);
            }
            EffectManager.sendUIEffect(22227, 2, player.CSteamID, true);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);
        }
    }
}
