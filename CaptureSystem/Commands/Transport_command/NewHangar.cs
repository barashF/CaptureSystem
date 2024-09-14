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

namespace CapturSystem.Commands.Transport_command
{
    class NewHangar : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "newhangar" };

        public List<string> Permissions => new List<string> { "admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 3)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /newhangar [id] [name] [radius]", UnityEngine.Color.red);
                return;
            }

            string id = command[0];
            string name = command[1].Replace("_", " ");
            float radius = (float)int.Parse(command[2]);

            CaptureSystem.Capture.test.Hangar.Add(new CaptureSystem.Hangar
            {
                id = id,
                name = name,
                radius = radius,
                point = player.Position,
                pointSpawnTransports = new List<CaptureSystem.PointSpawnTransport> { },
                transportInHangars = new List<CaptureSystem.TransportInHangar> { }
            });
            CaptureSystem.DB.DataBase.Save(CaptureSystem.Capture.test);
            UnturnedChat.Say(player, $"Ангар {id} успешно создан", UnityEngine.Color.yellow);
        }
    }
}
