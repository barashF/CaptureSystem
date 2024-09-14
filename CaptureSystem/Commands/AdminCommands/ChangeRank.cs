using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;


namespace CaptureSystem.Commands.AdminCommands
{
    class ChangeRank : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "changerank" };

        public List<string> Permissions => new List<string> { "admin" };

        public Views.Connected connected = new Views.Connected { };


        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer admin = (UnturnedPlayer)caller;
            if (command.Length != 2)
            {
                UnturnedChat.Say(admin, "Неверня структура команды, пример: /changerank [player name] [new rank]", UnityEngine.Color.red);
                return;
            }
            
            UnturnedPlayer player = UnturnedPlayer.FromName(command[0]);
            if(player == null)
            {
                UnturnedChat.Say(admin, "Игрок не найден", UnityEngine.Color.red);
                return;
            }

            var playerinf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            if (playerinf == null)
            {
                UnturnedChat.Say(admin, "Игрок не зарегистрирован в системе, ему нужно присоединиться к одной из команд", UnityEngine.Color.red);
                return;
            }

            int number_rank = int.Parse(command[1]) - 1;
            var rank = Capture.test.Rank.Find(r => r.pk == number_rank);
            if(rank == null)
            {
                UnturnedChat.Say(admin, "Ранг не зарегистрирован", UnityEngine.Color.red);
                return;
            }

            playerinf.rang = number_rank;
            DB.DataBase.Save(Capture.test);
        }
    }
}
