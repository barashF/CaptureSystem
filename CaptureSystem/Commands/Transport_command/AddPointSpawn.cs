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
    class AddPointSpawn : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "addpointspawn" };

        public List<string> Permissions => new List<string> { "admin" };
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 2)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /addpointspawn [id hangar] [classification]", UnityEngine.Color.red);
                return;
            }

            string id_hangar = command[0];
            var hangar = CaptureSystem.Capture.test.Hangar.Find(hang => hang.id == id_hangar);
            if (hangar == null)
            {
                UnturnedChat.Say(player, "Ангар с таким id не зарегистрирован", UnityEngine.Color.red);
                return;
            }

            CaptureSystem.Capture.test.Hangar.Find(hang => hang.id == id_hangar).pointSpawnTransports.Add(new CaptureSystem.PointSpawnTransport
            {
                classification = command[1],
                point = player.Position,
                quaternion = player.Player.transform.rotation
            });
            CaptureSystem.DB.DataBase.Save(CaptureSystem.Capture.test);
            UnturnedChat.Say(player, $"Точка спавна техники успешно добавлена", UnityEngine.Color.yellow);
        }
    }
}
