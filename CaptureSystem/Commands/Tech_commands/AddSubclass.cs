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
    class AddSubclass : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "addsubclass" };

        public List<string> Permissions => new List<string> { "admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 4)
            {
                UnturnedChat.Say(player, "Неверная структура команды, пример: /addsubclass [id] [team id] [name] [need rank]", UnityEngine.Color.red);
                return;
            }

            string id = command[0];
            string team_id = command[1];
            string name = command[2].Replace("_", " ");
            int need_rank = int.Parse(command[3]);

            Capture.test.Subclass.Add(new Subclass
            {
                id = id,
                team = team_id,
                name = name,
                need_rank = need_rank,
                image = ""
            });
            DB.DataBase.Save(Capture.test);

            UnturnedChat.Say(player, $"Подкласс успешно добавлен, {id}");
        }
    }
}
