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
    class UpdateRadius : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "updateradius" };

        public List<string> Permissions => new List<string> { "admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 1)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /updateradius [id]", UnityEngine.Color.red);
                return;
            }
            if (CaptureSystem.Capture.test.Location.Find(loc => loc.id == command[0]) == null)
            {
                UnturnedChat.Say(player, "Локации с таким id не существует", UnityEngine.Color.red);
                return;
            }

            float radius = Vector3.Distance(CaptureSystem.Capture.test.Location.Find(loc => loc.id == command[0]).point, player.Position);
            CaptureSystem.Capture.test.Location.Find(loc => loc.id == command[0]).radius = radius;
            CaptureSystem.DB.DataBase.Save(CaptureSystem.Capture.test);

            UnturnedChat.Say(player, "Радиус задан", UnityEngine.Color.yellow);
        }
    }
}
