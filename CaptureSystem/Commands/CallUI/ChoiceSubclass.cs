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
    class ChoiceSubclass : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "subclass" };

        public List<string> Permissions => new List<string> { "subclass" };

        public ControlUI ui = new ControlUI { };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            var playerInf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);

            var subclasses = Capture.test.Subclass.FindAll(subclass => subclass.team == playerInf.team);
            EffectManager.sendUIEffect(22230, 1, player.CSteamID, true);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);

            for (int i = 0; i < 6; i++)
            {
                EffectManager.sendUIEffectVisibility(1, player.CSteamID, false, "subclass_" + (i + 1).ToString(), false);
            }

            for (int i = 0; i < subclasses.Count; i++)
            {
                EffectManager.sendUIEffectVisibility(1, player.CSteamID, true, "subclass_" + (i + 1).ToString(), true);
                EffectManager.sendUIEffectText(1, player.CSteamID, true, "subclass_name_" + (i + 1).ToString(), subclasses[i].name);
                EffectManager.sendUIEffectText(1, player.CSteamID, true, "subclass_rank_" + (i + 1).ToString(), $"Ранг {subclasses[i].need_rank}");
            }

            Capture.buttonsSubclasses.RemoveAll(button => button.player == player.CSteamID);
            Capture.buttonsSubclasses.Add(new Capture.ButtonsSubclasses
            {
                player = player.CSteamID,
                subclasses = subclasses
            });
        }
    }
}
