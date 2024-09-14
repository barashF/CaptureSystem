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
    class AddImageFraction : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "addimagefraction" };

        public List<string> Permissions => new List<string> { "admin" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length != 2)
            {
                UnturnedChat.Say(player, "Неверня структура команды, пример: /addimagefraction [id] [path]", UnityEngine.Color.red);
                return;
            }

            var fraction = Capture.test.Fraction.Find(fr => fr.id == command[0]);
            if(fraction == null)
            {
                UnturnedChat.Say(player, "фракция с данным id не найдена", UnityEngine.Color.red);
                return;
            }

            fraction.image = command[1];
            DB.DataBase.Save(Capture.test);

            UnturnedChat.Say(player, "Иконка для фракции успешно установлена");
        }
    }
}
