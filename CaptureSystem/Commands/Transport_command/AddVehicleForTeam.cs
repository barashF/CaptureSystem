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
    class AddVehicleForTeam : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "addvehicleforteam" };

        public List<string> Permissions => new List<string> { "admin" };
        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 4)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /addvehicleforteam [id] [team] [base_amount] [supply]", UnityEngine.Color.red);
                return;
            }

            ushort id = (ushort)int.Parse(command[0]);
            string team = command[1];
            int base_amount = int.Parse(command[2]);
            int supply = int.Parse(command[3]);

            if(CaptureSystem.Capture.test.Transport.Find(vehicle => vehicle.id == id) == null)
            {
                UnturnedChat.Say(player, "Транспорт с таким id не зарегистрирован", UnityEngine.Color.red);
            }

            CaptureSystem.Capture.test.TransportInHangar.Add(new CaptureSystem.TransportInHangar
            {
                id = id,
                team = team,
                amount = base_amount,
                amount_base = base_amount,
                supply = supply,
                existence = base_amount
            });
            CaptureSystem.DB.DataBase.Save(CaptureSystem.Capture.test);
            UnturnedChat.Say(player, $"{id} успешно добавлен для {team}", UnityEngine.Color.yellow);

        }
    }
}
