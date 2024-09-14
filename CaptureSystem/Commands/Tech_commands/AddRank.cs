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

namespace CaptureSystem.Commands.Tech_commands
{
    class AddRank : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "addrank" };

        public List<string> Permissions => new List<string> { "admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 2)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /addrank [name] [rank score]", UnityEngine.Color.red);
                return;
            }

            string name = command[0].Replace("_", " ");
            int rank = int.Parse(command[1]);
            int pk;
            if (rank == 0)
            {
                pk = 0;
            }
            else
            {
                pk = Capture.test.Rank[Capture.test.Rank.Count - 1].pk + 1;
            }

            Capture.test.Rank.Add(new Rank
            {
                name = name,
                score = rank,
                pk = pk
            });
            DB.DataBase.Save(Capture.test);

            UnturnedChat.Say(player, "Звание добавлено", UnityEngine.Color.yellow);
        }
    }
}
