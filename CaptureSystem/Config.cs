using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureSystem
{
    public class Config : IRocketPluginConfiguration
    {
        public void LoadDefaults()
        {
            
            infantryA = "infa";
            infantryB = "infb";

            tankA = "tanka";
            tankB = "tankb";

            air_forceA = "airforcea";
            air_forceB = "airforceb";

            marincesA = "marincesa";
            marincesB = "marincesb";

            sfA = "sfa";
            sfB = "sfb";

            min_amount_capture = 3;
            time = 30;
            min_rang = 2;
            kd = 120;

            plus_score_for_kill = 2;
        }

        public List<string> hangars_commands_perms;

        public string infantryA;
        public string infantryB;

        public string tankA;
        public string tankB;

        public string air_forceA;
        public string air_forceB;

        public string marincesA;
        public string marincesB;

        public string sfA;
        public string sfB;

        public int min_amount_capture;
        public int time;
        public int min_rang;
        public int kd;

        public int plus_score_for_kill;
    }
}
