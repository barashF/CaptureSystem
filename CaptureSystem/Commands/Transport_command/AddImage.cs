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
    class AddImage : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "addimage" };

        public List<string> Permissions => new List<string> { "admin" };
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 2)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /addimage [id] [url image]", UnityEngine.Color.red);
                return;
            }

            ushort id = (ushort)int.Parse(command[0]);
            var vehicle = CaptureSystem.Capture.test.Transport.Find(transport => transport.id == id);
            if (vehicle == null)
            {
                UnturnedChat.Say(player, "Техника с таким id не зарегистрирована", UnityEngine.Color.red);
                return;
            }

            CaptureSystem.Capture.test.TransportImage.Add(new CaptureSystem.TransportImage
            {
                id = id,
                url_image = command[1]
            });
            CaptureSystem.DB.DataBase.Save(CaptureSystem.Capture.test);
            UnturnedChat.Say(player, $"Изображение техники успешно установлено", UnityEngine.Color.yellow);
        }
    }
}
