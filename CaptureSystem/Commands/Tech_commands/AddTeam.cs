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
    class AddTeam : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "addteam" };

        public List<string> Permissions => new List<string> { "admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 2)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /addteam [id] [name]", UnityEngine.Color.red);
                return;
            }

            string name = command[1].Replace("_", " ");
            Steamworks.CSteamID group_id = GroupManager.generateUniqueGroupID();
            var group = GroupManager.addGroup(group_id, name);

            Capture.test.Team.Add(new Team
            {
                id = command[0],
                name = name,
                point = player.Position,
                group_id = group_id
            });
            DB.DataBase.Save(Capture.test);

            UnturnedChat.Say(player, "Команда добавлена", UnityEngine.Color.yellow);
        }
    }
}
