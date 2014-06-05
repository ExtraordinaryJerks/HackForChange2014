using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using HackForChang2014Web.Models;

namespace HackForChang2014Web
{
    public class InfoDataHub : Hub
    {
        public void Send(TemperatureRecord temperature, LightLevelRecord lightLevel)
        {
        }

        public static void SendTemperature(TemperatureRecord temperature)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<InfoDataHub>();
            hubContext.Clients.All.sendTemperature(Convert.ToInt32(temperature.Temperature).ToString());
        }

        public static void SendLightLevel(LightLevelRecord lightLevel)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<InfoDataHub>();
            hubContext.Clients.All.sendLightLevel(lightLevel.LightLevel.ToString());
        }

        public static void SendCrowdAverage(int average)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<InfoDataHub>();
            hubContext.Clients.All.sendCrowdAverage(average.ToString());
        }
        public static void SendCrowdCountTotal(int total)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<InfoDataHub>();
            hubContext.Clients.All.sendCrowdCountTotal(total);
        }
    }
}