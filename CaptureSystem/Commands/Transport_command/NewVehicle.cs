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
    class NewVehicle : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "newvehicle" };

        public List<string> Permissions => new List<string> { "admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length < 5)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /newvehicle [id] [name] [classification] [kd] [rang] [id_item1] [amount item1]", UnityEngine.Color.red);
                return;
            }

            ushort id = (ushort)int.Parse(command[0]);
            string name = command[1].Replace("_", " ");
            string classification = command[2];
            int kd = int.Parse(command[3]);
            int rang = int.Parse(command[4]);

            ushort item_id = 0;
            int amount_item = 0;
            List<CaptureSystem.ItemAndAmount> items = new List<CaptureSystem.ItemAndAmount> { };
            for(int i = 5; i < command.Length; i++)
            {
                if(i % 2 != 0)
                {
                    item_id = (ushort)int.Parse(command[i]);
                }
                else
                {
                    amount_item = int.Parse(command[i]);
                    items.Add(new CaptureSystem.ItemAndAmount { amount = amount_item, id = item_id });
                }
            }

            CaptureSystem.Capture.test.Transport.Add(new CaptureSystem.Transport
            {
                id = id,
                name = name,
                classification = classification,
                kd = kd,
                rang = rang,
                set = items
            });
            CaptureSystem.DB.DataBase.Save(CaptureSystem.Capture.test);
            UnturnedChat.Say(player, $"Транспорт успешно добавлен в систему: {id}, {name}", UnityEngine.Color.yellow);
        }
    }
}
