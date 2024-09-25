using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;


namespace CaptureSystem.Views
{
    public class Pull
    {
        public static Transform Raycast(IRocketPlayer rocketPlayer)
        {
            RaycastHit hit;
            UnturnedPlayer player = (UnturnedPlayer)rocketPlayer;
            if (Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out hit, 1, RayMasks.BARRICADE_INTERACT))
            {
                Transform transform = hit.transform;


                return transform;
            }
            return null;
        }

        public void TryStartPull(UnturnedPlayer pullPLayer)
        {
            var hit = Raycast(pullPLayer);
            if(hit == null)
            {
                return;
            }

            try
            {
                var hitPlayer = Raycast(player).GetComponent<UnturnedPlayer>();
            }
            catch
            {
                return;
            }
            
            var knockedPlayer = knockedOutPlayers.Find(knockedPlayer => kn.player == hitPlayer.CSteamID);
            if(knockedPlayer == null)
            {
                return;
            }

            Capture.onPulledPlayers.Add(new PullPlayer
            {
                knockedPlayer = knockedPlayer,
                pullPLayer = pullPLayer
            })

        }
        

        public void TryFinishPull(UnturnedPlayer pullPlayer)
        {
            Capture.onPulledPlayers.RemoveAll(pull => pull.pullPlayer == pullPlayer || pull.knockedPlayer == pullPlayer);
            return;
        }

        public void TryPull(UnturnedPlayer pullPlayer)
        {
            var pulled = Capture.onPulledPlayers.Find(pl => pl.pullPlayer == pullPlayer);
            if(pulled == null)
            {
                return;
            }

            var knockedPlayer = UnturnedPlayer.FromCSteamID(pulled.knockedPlayer);
            knockedPlayer.Teleport(pullPlayer.Position, knockedPlayer.Player.transform.rotation.x);
        }
    }
}