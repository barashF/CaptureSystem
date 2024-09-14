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
    public class Knock
    {
        public void TryKillKnocked()
        {
            for (int i = Capture.knockedOutPlayers.Count - 1; i >= 0; i--)
            {
                var playerData = Capture.knockedOutPlayers[i];
                playerData.time -= 1;
                if (playerData.time < 1)
                {
                    
                    UnturnedPlayer player = UnturnedPlayer.FromCSteamID(playerData.player);
                    player.Player.life.askDamage((byte)(player.Player.life.health + 1), Vector3.zero, EDeathCause.GUN, ELimb.SKULL, CSteamID.Nil, out _);

                    Capture.knockedOutPlayers.RemoveAt(i);
                }
            }
        }


        public void TryKnockPLayer(DamagePlayerParameters parameters)
        {
            UnturnedPlayer player = UnturnedPlayer.FromPlayer(parameters.player);
            var hit = parameters.limb;

            var knocked = Capture.knockedOutPlayers.Find(k => k.player == player.CSteamID);
            System.Random random = new System.Random { };
            int value = random.Next(1, 101);
            if (hit.ToString() == "SKULL" & value < 31)
            {
                player.Player.stance.stance = EPlayerStance.PRONE;
                player.Player.equipment.dequip();
                Capture.knockedOutPlayers.Add(new Capture.KnockedOutPlayer
                {
                    player = player.CSteamID,
                    time = 180
                });
            }
        }
    }
}
