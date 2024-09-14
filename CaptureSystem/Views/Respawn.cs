using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Unturned.Player;
using Rocket.API;

namespace CaptureSystem.Views
{
    public class Respawn
    {
        public void OnRespawn(UnturnedPlayer player)
        {
            var playerinf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            if(playerinf == null)
            {
                return;
            }

            var team = Capture.test.Team.Find(t => t.id == playerinf.team);
            player.Teleport(team.point, player.Rotation);
        }
    }
}
