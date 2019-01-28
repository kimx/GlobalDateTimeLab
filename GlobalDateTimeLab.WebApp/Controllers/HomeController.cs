﻿using GlobalDateTimeLab.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlobalDateTimeLab.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Send(HomeModel model)
        {
            model.CreateDateTime = DateTime.UtcNow.AddHours(8);
            model.CreateUtcDateTime = DateTime.UtcNow;
            return new ScJsonResult(model, model.UseUnSpecified);
        }

        public ActionResult TimeTable()
        {
            return View();
        }

        public ActionResult SgForm()
        {
            return View();
        }
    }
}