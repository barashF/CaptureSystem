using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Unturned.Player;
using Rocket.Core.Permissions;
using Rocket.Core;
using Rocket.Unturned.Chat;
using SDG.Unturned;


namespace CaptureSystem.Views
{
    public class Fractions
    {
        public List<Fraction> GetPlayerFraction(UnturnedPlayer player)
        {
            var playerInf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            var fractions = Capture.test.Fraction.FindAll(frac => frac.team == playerInf.team);
            RocketPermissionsManager permissionsManager = (RocketPermissionsManager)R.Permissions;

            var permissions = permissionsManager.GetGroups(player, false);
            List<string> permissionId = new List<string> { };
            foreach (var i in permissions)
            {
                permissionId.Add(i.Id);
            }

            List<Fraction> playerFractions = new List<Fraction> { };
            foreach(var i in fractions)
            {
                if (permissionId.Contains(i.id))
                {
                    playerFractions.Add(i);
                }
            }
            return playerFractions;
        }


        public void RemovePlayerFractions(UnturnedPlayer player, List<Fraction> fractions)
        {
            RocketPermissionsManager permissionsManager = (RocketPermissionsManager)R.Permissions;
            foreach (var i in fractions)
            {
                permissionsManager.RemovePlayerFromGroup(i.id, player);
            }
        }


        public void ClearUIFractions(UnturnedPlayer player)
        {
            EffectManager.askEffectClearByID(22229, player.CSteamID);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
        }


        public void ChangeFraction(UnturnedPlayer player, string fractionId)
        {
            var fractions = GetPlayerFraction(player);
            foreach(var i in fractions)
            {
                if(i.id == fractionId)
                {
                    UnturnedChat.Say(player, "Вы уже состоите в данной фракции", UnityEngine.Color.red);
                    ClearUIFractions(player);
                    return;
                }
            }
            RemovePlayerFractions(player, fractions);

            var playerInf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            var fraction = Capture.test.Fraction.Find(fr => fr.id == fractionId);

            if(playerInf.rang < fraction.need_rank)
            {
                UnturnedChat.Say(player, "У вас слишком низкий ранг", UnityEngine.Color.red);
                ClearUIFractions(player);
                return;
            }

            RocketPermissionsManager permissionsManager = (RocketPermissionsManager)R.Permissions;
            var group = permissionsManager.GetGroup(fractionId);
            var result = permissionsManager.AddPlayerToGroup(group.Id, player);
            ClearUIFractions(player);
        }
    }
}
