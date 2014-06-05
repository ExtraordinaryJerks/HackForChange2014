using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackForChang2014Web.Models
{
    public class CrowdCountRecord
    {
        public ObjectId _id { get; set; }
        public string TimeToTheMinute { get; set; }

        public int Count { get; set; }

        public DateTime TheTimeStamp { get; set; }
    }
}