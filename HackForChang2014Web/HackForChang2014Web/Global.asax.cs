using HackForChang2014Web.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MongoDB.Driver.Linq;
namespace HackForChang2014Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        MongoCollection<CrowdCountRecord> theCollection;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Timer timer = new Timer(3000);
            timer.Elapsed += timer_Elapsed;
            theCollection = GetDatabase().GetCollection<CrowdCountRecord>("CrowdCountRecord");
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var theLastFifteenMinutes = theCollection.AsQueryable()
                            .OrderByDescending(record => record.TimeToTheMinute)
                           .Where(record => record.TheTimeStamp > DateTime.Now.AddMinutes(-15) && record.TheTimeStamp < DateTime.Now.AddSeconds(1))
                           .ToList();
                if (theLastFifteenMinutes.Count > 0)
                {
                    var average = theLastFifteenMinutes.Average(record => record.Count);
                    InfoDataHub.SendCrowdAverage(Convert.ToInt32(average));
                }

                var total = 0;
                theCollection.AsQueryable()
                     .Where(record => record.TheTimeStamp > DateTime.Now.AddHours(-24) && record.TheTimeStamp < DateTime.Now.AddSeconds(1))
                     .ToList()
                     .ForEach(record =>
                     {
                         total += record.Count;
                     });
                InfoDataHub.SendCrowdCountTotal(total);
            }
            catch (Exception ex)
            {

            }
        }

        private MongoDatabase GetDatabase()
        {
            MongoClient client = new MongoClient("mongodb://localhost");
            var server = client.GetServer();
            var database = server.GetDatabase("HackForChange");
            return database;
        }
    }
}
