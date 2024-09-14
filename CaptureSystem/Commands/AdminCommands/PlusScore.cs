using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using SDG.Unturned;

namespace CaptureSystem.Commands.AdminCommands
{
    class PlusScore : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "plusscore" };

        public List<string> Permissions => new List<string> { "admin" };

        public Views.Score scor_view = new Views.Score { };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;


            scor_view.UpdateScore(player, int.Parse(command[0]));
        }
    }
}
