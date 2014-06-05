using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackForChang2014Web.Models
{
    public class ParkView
    {
        public Park Park { get; set; }
        public List<Police> PoliceStations { get; set; }
        public List<FireStation> FireStattions { get; set; }
    }
}