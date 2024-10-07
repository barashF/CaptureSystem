using Rocket.API;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using Rocket.API.Collections;
using Rocket.Unturned.Chat;
using Logger = Rocket.Core.Logging.Logger;
using System.Timers;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using System;
using SDG.NetTransport;
using System.Threading;
using Steamworks;

namespace CaptureSystem.Views
{
    public class Score
    {
        public void TryUpdateRank(UnturnedPlayer player)
        {
            var playerinf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            Rank next_rank;
            try
            {
                next_rank = Capture.test.Rank[playerinf.rang + 1];
            }
            catch
            {
                EffectManager.sendUIEffectText(5, player.CSteamID, true, "hud_rank", "");
                return;
            }

            Rank rank;
            if(playerinf.score >= next_rank.score)
            {
                playerinf.rang += 1;
                DB.DataBase.Save(Capture.test);

                EffectManager.sendUIEffectText(5, player.CSteamID, true, "rank", next_rank.name);
                EffectManager.sendUIEffectText(5, player.CSteamID, true, "score", playerinf.score.ToString() + "/" + next_rank.score.ToString());
                rank = Capture.test.Rank[playerinf.rang];
                try
                {
                    next_rank = Capture.test.Rank[rank.pk + 1];
                }
                catch
                {
                    EffectManager.sendUIEffectText(5, player.CSteamID, true, "hud_rank", "");
                    return;
                }
                SendProgresBarRankUI(rank, next_rank, player);
            }
            else
            {
                rank = Capture.test.Rank[playerinf.rang];
                SendProgresBarRankUI(rank, next_rank, player);
            }
            
        }

        public void UpdateScore(UnturnedPlayer player, int plus_score)
        {
            var playerinf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            int next_rank;
            try
            {
                next_rank = Capture.test.Rank[playerinf.rang + 1].score;
            }
            catch
            {
                next_rank = 1000000;
            }

            if(playerinf == null)
            {
                return;
            }

            playerinf.score += plus_score;
            DB.DataBase.Save(Capture.test);
            EffectManager.sendUIEffectText(5, player.CSteamID, true, "score", playerinf.score.ToString() + "/" + next_rank.ToString());

            TryUpdateRank(player);
        }

        public void PlucScoreKill(UnturnedPlayer player, UnturnedPlayer killed)
        {
            try
            {
                var player_killer = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
                var player_killed = Capture.test.PlayerInf.Find(inf => inf.player == killed.CSteamID);

                if (player_killed.team != player_killer.team)
                {
                    UpdateScore(player, Capture.cfg.plus_score_for_kill);
                }
            }
            catch
            {
                return;
            }
        }

        public void SendProgresBarRankUI(Rank rank, Rank next_rank, UnturnedPlayer player)
        {
            var playerinf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            int range = next_rank.score - rank.score;
            int remainder = next_rank.score - playerinf.score;

            int percent = (remainder * 100) / range;
            float value_words = percent * 1.1f;

            string progress = "";
            for(int i = 0; i < (int)value_words; i++)
            {
                progress += "f";
            }

            EffectManager.sendUIEffectText(5, player.CSteamID, true, "hud_rank", progress);
        }

        public void SendRankUI(UnturnedPlayer player)
        {
            var playerinf = Capture.test.PlayerInf.Find(inf => inf.player == player.CSteamID);
            if(playerinf == null)
            {
                return;
            }

            EffectManager.sendUIEffect(22228, 5, player.CSteamID, true);
            var rank = Capture.test.Rank.Find(r => r.pk == playerinf.rang);
            try
            {
                var next_rank = Capture.test.Rank[rank.pk + 1];
                SendProgresBarRankUI(rank, next_rank, player);
                EffectManager.sendUIEffectText(5, player.CSteamID, true, "score", playerinf.score.ToString() + "/" + next_rank.score.ToString());
            }
            catch
            {
                EffectManager.sendUIEffectText(5, player.CSteamID, true, "hud_rank", "");
                EffectManager.sendUIEffectText(5, player.CSteamID, true, "score", playerinf.score.ToString() + "/" + "1000000");
            }
            
            EffectManager.sendUIEffectText(5, player.CSteamID, true, "rank", rank.name);
            
        }

        public void ScoreForCapt(string id_team, string id_location)
        {
            var playersonLocation = Capture.playerOnLocations.FindAll(loc => loc.id_location == id_location & loc.team == id_team);

            foreach(var i in playersonLocation)
            {
                UnturnedPlayer player = UnturnedPlayer.FromCSteamID(i.player);
                UpdateScore(player, 5);
            }
        }
    }
}
