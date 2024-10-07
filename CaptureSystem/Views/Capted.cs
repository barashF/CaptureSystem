using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureSystem.Views
{
    public class Capted
    {
        public void UpdateTimerCapt(Capture.CaptureLocation capt)
        {
            string loc_name = Capture.test.Location.Find(loc => loc.id == capt.id_location).name;
            string team_name = Capture.test.Team.Find(team => team.id == capt.team).name;
            string percent = PercentCapt(capt.time).ToString();

            foreach(var i in Capture.playerOnLocations.FindAll(pl => pl.id_location == capt.id_location))
            {
                EffectManager.sendUIEffect(22225, 3, i.player, true);
                EffectManager.sendUIEffectText(3, i.player, true, "Name", loc_name);
                EffectManager.sendUIEffectText(3, i.player, true, "Owner", $"{team_name} захватывает: {percent}%");
                
            }
        }

        public int PercentCapt(int time)
        {
            int progress = 900 - time;
            int percent = (progress * 100) / 900 ;
            return percent;
        }

        
    }
}
