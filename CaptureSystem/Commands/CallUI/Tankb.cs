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
    class Tankb : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "tanknato" };

        public List<string> Permissions => new List<string> { "tanknato" };

        public ControlUI ui = new ControlUI { };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            var vehicles = Capture.test.Hangar.Find(hang => hang.id == "tanknato").transportInHangars;
            EffectManager.sendUIEffect(22224, 1, player.CSteamID, true);
            ui.HideUI(player);
            ui.ShowBlocksUI(player, vehicles, "tanknato");
        }
    }
}
