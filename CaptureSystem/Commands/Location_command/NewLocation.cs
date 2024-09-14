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
    class NewLocation : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "newlocation" };

        public List<string> Permissions => new List<string> { "admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if(command.Length != 2)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /newlocation [id] [name]", UnityEngine.Color.red);
                return;
            }
            if(CaptureSystem.Capture.test.Location.Find(location => location.id == command[0]) != null)
            {
                UnturnedChat.Say(player, "Локация с таким id уже существует", UnityEngine.Color.red);
                return;
            }

            string name = command[1].Replace("_", " ");
            CaptureSystem.Capture.test.Location.Add(new CaptureSystem.Location
            {
                id = command[0],
                name = name,
                boosts = new List<CaptureSystem.Boost> { },
                point = player.Position,
                radius = 0,
                team = ""
            });

            CaptureSystem.DB.DataBase.Save(CaptureSystem.Capture.test);
            UnturnedChat.Say(player, $"Создана локация {name} с id: {command[0]}", UnityEngine.Color.yellow);

        }
    }
}
