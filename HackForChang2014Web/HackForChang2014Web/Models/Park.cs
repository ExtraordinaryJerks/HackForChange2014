using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackForChang2014Web.Models
{
    public class Park
    {
        public ObjectId _id { get; set; }

        public string Name { get; set; }
        public GeoJson2DGeographicCoordinates Location { get; set; }
    }
}
