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
    class ChoiceFraction : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "fractions" };

        public List<string> Permissions => new List<string> { "fractions" };

        public ControlUI ui = new ControlUI { };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            var playerInf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            var team = Capture.test.Team.Find(t => t.id == playerInf.team);
            if(Vector3.Distance(player.Position, team.point) > 500)
            {
                UnturnedChat.Say(player, "Вы находитесь слишком далеко от базы", UnityEngine.Color.red);
                return;
            }

            var fractions = Capture.test.Fraction.FindAll(fraction => fraction.team == playerInf.team);
            EffectManager.sendUIEffect(22229, 1, player.CSteamID, true);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);

            for (int i = 0; i < 4; i++)
            {
                EffectManager.sendUIEffectVisibility(1, player.CSteamID, false, "group_" + (i + 1).ToString(), false);
            }

            for(int i = 0; i < fractions.Count; i++)
            {
                EffectManager.sendUIEffectVisibility(1, player.CSteamID, true, "group_" + (i + 1).ToString(), true);
                EffectManager.sendUIEffectImageURL(1, player.CSteamID, true, "group_image_" + (i + 1).ToString(), fractions[i].image);
                EffectManager.sendUIEffectText(1, player.CSteamID, true, "group_name_" + (i + 1).ToString(), fractions[i].name);
                EffectManager.sendUIEffectText(1, player.CSteamID, true, "group_rang_" + (i + 1).ToString(), $"Необходимый ранг: {fractions[i].need_rank}");
            }

            Capture.buttonsFractions.RemoveAll(button => button.player == player.CSteamID);
            Capture.buttonsFractions.Add(new Capture.ButtonsFractions
            {
                player = player.CSteamID,
                fractions = fractions
            });
        }
    }
}
