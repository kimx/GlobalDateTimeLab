using GlobalDateTimeLab.Console.Lib;
using GlobalDateTimeLab.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
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

        public ActionResult Culture()
        {
            CultureModel model = new CultureModel();
            model.TimezoneHour = 0;
            model.CurrentCultureDateTime = DateTimeExtensions.GetCurrentCultureDateTime();
            HttpCookie timezoneHourCookie = System.Web.HttpContext.Current.Request.Cookies["timezoneHour"];
            if (timezoneHourCookie != null)
            {
                model.TimezoneHour = Convert.ToInt32(timezoneHourCookie.Value);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Culture(int timezoneHour)
        {
            Response.Cookies.Remove("timezoneHour");

            HttpCookie timezoneHourCookie = System.Web.HttpContext.Current.Request.Cookies["timezoneHour"];

            if (timezoneHourCookie == null)
                timezoneHourCookie = new HttpCookie("timezoneHour");
            timezoneHourCookie.Value = timezoneHour.ToString();
            timezoneHourCookie.Expires = DateTime.Now.AddDays(10);
            Response.SetCookie(timezoneHourCookie);
            return Redirect("Culture");
        }
    }
}