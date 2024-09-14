using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Rocket.Core.Permissions;
using Rocket.Core;
using Rocket.Unturned.Chat;


namespace CaptureSystem.Views
{
    public class Group
    {
        public Connected connected = new Connected { };
        public void ChangeTeam(UnturnedPlayer player, string team_id)
        {
            var team = Capture.test.Team.Find(t => t.id == team_id);
            connected.CreatePlayerInf(player, team);
            connected.TryInviteInGroup(player.CSteamID);

            RocketPermissionsManager permissionsManager = (RocketPermissionsManager)R.Permissions;
            var group = permissionsManager.GetGroup(team_id);
            var result = permissionsManager.AddPlayerToGroup(group.Id, player);
        }
    }
}
