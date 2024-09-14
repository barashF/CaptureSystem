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
    class AddBoostLocation : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "addboostlocation" };

        public List<string> Permissions => new List<string> { "admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 5)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /addboostlocation [id] [vehicle_A] [vehicle_B] [boost amount in hangar] [amount supply]", UnityEngine.Color.red);
                return;
            }
            if (CaptureSystem.Capture.test.Location.Find(loc => loc.id == command[0]) == null)
            {
                UnturnedChat.Say(player, "Локация с таким id не существует", UnityEngine.Color.red);
                return;
            }
            if(CheckRegTransport((ushort)int.Parse(command[1])) == false)
            {
                UnturnedChat.Say(player, $"Транспорт с id: {command[1]} не зарегистрирован", UnityEngine.Color.red);
                return;
            }
            if (CheckRegTransport((ushort)int.Parse(command[2])) == false)
            {
                UnturnedChat.Say(player, $"Транспорт с id: {command[2]} не зарегистрирован", UnityEngine.Color.red);
                return;
            }

            var location = CaptureSystem.Capture.test.Location.Find(loc => loc.id == command[0]);
            location.boosts.Add(new CaptureSystem.Boost
            {
                amount_storage = int.Parse(command[3]),
                supply = int.Parse(command[4]),
                teamA = (ushort)int.Parse(command[1]),
                teamB = (ushort)int.Parse(command[2])
            });
            CaptureSystem.DB.DataBase.Save(CaptureSystem.Capture.test);
            UnturnedChat.Say(player, CaptureSystem.Capture.test.Location.Find(loc => loc.id == command[0]).boosts.ToString(), UnityEngine.Color.yellow);
        }


        public bool CheckRegTransport(ushort id_vehicle)
        {
            var vehicle = CaptureSystem.Capture.test.Transport.Find(transport => transport.id == id_vehicle);
            if(vehicle != null)
            {
                return true;
            }
            return false;
        }
    }
}
