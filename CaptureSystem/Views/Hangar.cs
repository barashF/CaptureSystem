using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Unturned.Player;


namespace CaptureSystem.Views
{
    public class Hangar
    {
        public void AddPlayerVehicle(UnturnedPlayer player, ushort vehicle)
        {
            Capture.playerVehicles.RemoveAll(plveh => plveh.player == player.CSteamID);
            Capture.playerVehicles.Add(new Capture.PlayerVehicle
            {
                player = player.CSteamID,
                vehicle = vehicle,
                time = 1200
            });
        }

        public void UpdateKD()
        {
            foreach(var i in Capture.playerVehicles)
            {
                i.time = -1;
            }
        }
    }
}
