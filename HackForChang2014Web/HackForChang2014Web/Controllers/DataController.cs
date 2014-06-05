using HackForChang2014Web.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HackForChang2014Web.Controllers
{
    public class DataController : ApiController
    {
        // POST api/data
        public HttpResponseMessage Post([FromBody]string value)
        {
            //value = this.Request.Content.ReadAsStringAsync().Result;

            if (!string.IsNullOrEmpty(value))
            {
                var theValues = value.Split(',');

                TemperatureRecord temp = new TemperatureRecord();
                LightLevelRecord light = new LightLevelRecord();
                temp.Temperature = Convert.ToDecimal(theValues[0]);
                light.LightLevel = ConvertLightToOneToHunderRangeValue(Convert.ToInt32(theValues[1]));
                bool isCountIncremented = false;
                if (!string.IsNullOrEmpty(theValues[2]) && Convert.ToInt32(theValues[2]) > 0)
                    isCountIncremented = true;


                var theDatabase = GetDatabase();

                InfoDataHub.SendTemperature(temp);
                InfoDataHub.SendLightLevel(light);

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        theDatabase.GetCollection<TemperatureRecord>("TemperatureRecord").Insert(temp);
                        theDatabase.GetCollection<LightLevelRecord>("LightLevelRecord").Insert(light);
                        if (isCountIncremented)
                        {
                            var theTimeToTheMinute = string.Format("{0:yyyy-MM-dd HH:mm}", DateTime.Now);
                            var update = Update.Inc("Count", 1).Set("TheTimeStamp", DateTime.Now);
                            theDatabase.GetCollection<CrowdCountRecord>("CrowdCountRecord").Update(Query.EQ("TimeToTheMinute", theTimeToTheMinute), update, UpdateFlags.Upsert);
                        }
                    }
                    catch (Exception ex) { }
                });

                return this.Request.CreateResponse(HttpStatusCode.Accepted);
            }

            return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        private int ConvertLightToOneToHunderRangeValue(int oldlightLevel)
        {
            //light value is coming from sensor where 255 is dark and 0 is completely light
            //this function will convert 0 to dark and 100 to lightest
            var oldRange = (255 - 0);
            int newValue = 0;
            int newRange = 0;
            if (oldRange == 0)
                newValue = 0;
            else
            {
                newRange = (100 - 0);
                newValue = (((oldlightLevel - 0) * newRange) / oldRange) + 0;
            }

            newValue = (newValue * -1) + 100;

            return newValue;
        }

        public MongoDatabase GetDatabase()
        {
            MongoClient client = new MongoClient("mongodb://localhost");
            var server = client.GetServer();
            var database = server.GetDatabase("HackForChange");
            return database;
        }
    }
}
