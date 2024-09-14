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
    class ChangeScore : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "changescore" };

        public List<string> Permissions => new List<string> { "admin" };

        public Views.Score scor_view = new Views.Score { };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer admin = (UnturnedPlayer)caller;
            if (command.Length != 2)
            {
                UnturnedChat.Say(admin, "Неверня структура команды, пример: /changescore [player name] [new score]", UnityEngine.Color.red);
                return;
            }

            UnturnedPlayer player = UnturnedPlayer.FromName(command[0]);
            if (player == null)
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

            int score = int.Parse(command[1]);
            playerinf.score = score;
            DB.DataBase.Save(Capture.test);

            scor_view.TryUpdateRank(player);
        }
    }
}
