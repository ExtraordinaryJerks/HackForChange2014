using HackForChang2014Web.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HackForChang2014Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var theParks = GetDatabase().GetCollection<Park>("Parks").FindAll().ToList();

            return View(theParks);
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
