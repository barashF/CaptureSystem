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
    class AddVehicleInHangar : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "addvehicleinhangar" };

        public List<string> Permissions => new List<string> { "admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 3)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /addvehicleinhangar [id hangar] [team] [id vehicle]", UnityEngine.Color.red);
                return;
            }

            string id_hangar = command[0];
            string team = command[1];
            ushort id_vehicle = (ushort)int.Parse(command[2]);
            var transport_team = CaptureSystem.Capture.test.TransportInHangar.Find(transport => transport.id == id_vehicle & transport.team == team);
            if(transport_team == null)
            {
                UnturnedChat.Say(player, "Техника не зарегистрирована за эту команду", UnityEngine.Color.red);
                return;
            }

            var hangar = CaptureSystem.Capture.test.Hangar.Find(hang => hang.id == id_hangar);
            if(hangar == null)
            {
                UnturnedChat.Say(player, "Ангар с таким id не зарегистрирован", UnityEngine.Color.red);
                return;
            }

            CaptureSystem.Capture.test.Hangar.Find(hang => hang.id == id_hangar).transportInHangars.Add(transport_team);
            CaptureSystem.DB.DataBase.Save(CaptureSystem.Capture.test);

            UnturnedChat.Say(player, $"Техника успешно добавлена в ангар", UnityEngine.Color.yellow);
        }
    }
}
