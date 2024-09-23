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
using Rocket.Core.Permissions;
using Rocket.Core;

namespace CaptureSystem.Commands.CallUI
{
    class ChangeTeam : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "changeteam" };

        public List<string> Permissions => new List<string> { "changeteam" };

        public ControlUI ui = new ControlUI { };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            RocketPermissionsManager permissionsManager = (RocketPermissionsManager)R.Permissions;
            permissionsManager.RemovePlayerFromGroup("RF", player);
            permissionsManager.RemovePlayerFromGroup("NATO", player);
            EffectManager.sendUIEffect(22227, 2, player.CSteamID, true);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);
        }
    }
}
