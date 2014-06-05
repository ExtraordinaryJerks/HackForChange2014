using HackForChang2014Web.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HackForChang2014Web.Controllers
{
    public class ParkController : Controller
    {
        //
        // GET: /Park/
        public ActionResult Index(string name)
        {
            var park = GetDatabase().GetCollection<Park>("Parks").FindOne(Query.EQ("Name", name));
            var parkView = new ParkView();
            parkView.Park = park;
            if (park.Location != null)
            {
                parkView.PoliceStations = GetPolice(park.Location.Longitude, park.Location.Latitude);
                parkView.FireStattions = GetFireStations(park.Location.Longitude, park.Location.Latitude);
            }
            else
            {
                parkView.PoliceStations = new List<Police>();
                parkView.FireStattions = new List<FireStation>();
            }

            return View(parkView);
        }

        private List<Police> GetPolice(double longitude, double latitude)
        {
            double radius = 8046;
            var policeCollection = GetDatabase().GetCollection<Police>("Police");
            var centerPoint = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(longitude, latitude));
            var query = Query.Near("Location", centerPoint, radius, true);
            var queryResolved = policeCollection.Find(query);
            return queryResolved.ToList<Police>();
        }

        private List<FireStation> GetFireStations(double longitude, double latitude)
        {
            double radius = 8046;
            var fireCollection = GetDatabase().GetCollection<FireStation>("FireStations");
            var centerPoint = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(longitude, latitude));
            var query = Query.Near("Location", centerPoint, radius, true);
            var queryResolved = fireCollection.Find(query);
            return queryResolved.ToList<FireStation>();
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