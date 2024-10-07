using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Rocket.Unturned.Chat;


namespace CaptureSystem.Views
{
    public class Connected
    {
        public Team TryGetTeam(CSteamID player)
        {
            PlayerInf inf = Capture.test.PlayerInf.Find(plinf => plinf.player == player);
            if(inf == null)
            {
                return null;
            }

            Team team = Capture.test.Team.Find(t => t.id == inf.team);
            return team;
        }

        public void TryTellUIStart(CSteamID player)
        {
            Team team = TryGetTeam(player);
            if (team == null)
            {
                EffectManager.sendUIEffect(22227, 2, player, true);
                UnturnedPlayer.FromCSteamID(player).Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);
            }
        }

        public void JoinInGroup(UnturnedPlayer uplayer, Team team)
        {
            uplayer.Player.quests.sendAddGroupInvite(team.group_id);
            uplayer.Player.quests.ReceiveAcceptGroupInvitationRequest(team.group_id);
        }

        public void TryInviteInGroup(CSteamID player)
        {
            UnturnedPlayer uplayer = UnturnedPlayer.FromCSteamID(player);
            Team team = TryGetTeam(player);
            if(team == null)
            {
                return;
            }

            JoinInGroup(uplayer, team);
            uplayer.Teleport(team.point, uplayer.Rotation);
        }

        public void CreatePlayerInf(UnturnedPlayer player, Team team)
        {
            Capture.test.PlayerInf.RemoveAll(inf => inf.player == player.CSteamID);
            DB.DataBase.Save(Capture.test);

            Capture.test.PlayerInf.Add(new PlayerInf
            {
                team = team.id,
                player = player.CSteamID,
                rang = 0,
                score = 0
            });
            DB.DataBase.Save(Capture.test);
        }

        public void AddOnlinePlayer(UnturnedPlayer player)
        {
            var playerInf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            if(playerInf == null)
            {
                return;
            }
            try
            {
                Capture.onServerPlayers.Add(playerInf);
            }
            catch { }
        }
    }
}
