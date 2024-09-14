using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Core;
using Rocket.Core.Permissions;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace CaptureSystem.Views
{
    public class Subclasses
    {
        public List<Subclass> GetPlayerSubclass(UnturnedPlayer player)
        {
            var playerInf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            var subclasses = Capture.test.Subclass.FindAll(frac => frac.team == playerInf.team);
            RocketPermissionsManager permissionsManager = (RocketPermissionsManager)R.Permissions;

            var permissions = permissionsManager.GetGroups(player, false);
            List<string> permissionId = new List<string> { };
            foreach (var i in permissions)
            {
                permissionId.Add(i.Id);
            }

            List<Subclass> playerSubclasses = new List<Subclass> { };
            foreach (var i in subclasses)
            {
                if (permissionId.Contains(i.id))
                {
                    playerSubclasses.Add(i);
                }
            }
            return playerSubclasses;
        }

        public void RemovePlayerSubclass(UnturnedPlayer player, List<Subclass> playerSubclasses)
        {
            RocketPermissionsManager permissionsManager = (RocketPermissionsManager)R.Permissions;
            foreach (var i in playerSubclasses)
            {
                permissionsManager.RemovePlayerFromGroup(i.id, player);
            }
        }

        public void ClearUISubclass(UnturnedPlayer player)
        {
            EffectManager.askEffectClearByID(22230, player.CSteamID);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
        }


        public void ChangeSubclass(UnturnedPlayer player, string subclassId)
        {
            var subclasses = GetPlayerSubclass(player);
            foreach (var i in subclasses)
            {
                if (i.id == subclassId)
                {
                    UnturnedChat.Say(player, "Вы уже выбрали этот подкласс", UnityEngine.Color.red);
                    ClearUISubclass(player);
                    return;
                }
            }
            RemovePlayerSubclass(player, subclasses);

            var playerInf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            var subclass = Capture.test.Subclass.Find(fr => fr.id == subclassId);

            if (playerInf.rang < subclass.need_rank)
            {
                UnturnedChat.Say(player, "У вас слишком низкий ранг", UnityEngine.Color.red);
                ClearUISubclass(player);
                return;
            }

            RocketPermissionsManager permissionsManager = (RocketPermissionsManager)R.Permissions;
            var group = permissionsManager.GetGroup(subclassId);
            var result = permissionsManager.AddPlayerToGroup(group.Id, player);
            ClearUISubclass(player);
        }
    }
}
