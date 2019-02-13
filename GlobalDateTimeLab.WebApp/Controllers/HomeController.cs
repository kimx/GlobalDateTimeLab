using GlobalDateTimeLab.Console.Lib;
using GlobalDateTimeLab.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GlobalDateTimeLab.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string userNo = "Kim";
            string s = null;
            short? t = null;
            string c = $"{userNo};{s};{t}";
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
            CustomCultureInfo customCultureInfo = Thread.CurrentThread.CurrentCulture as CustomCultureInfo;
            CultureModel model = new CultureModel();
            model.TimezoneHour = 0;
            model.CurrentCultureDateTime = DateTimeExtensions.GetCustomCultureDateTime();
            model.PricipleDateTime = DateTimeExtensions.GetUserThreadPricipleDateTime();
            if (customCultureInfo != null)
                model.TimezoneHour = customCultureInfo.UtcHours;

            model.Messages.Add(System.Web.HttpContext.Current.Items["culture-begin"] as string);
            model.Messages.Add(System.Web.HttpContext.Current.Items["culture-end"] as string);

            model.Messages.Add($"Home-UtcHours:{customCultureInfo?.UtcHours}  : {DateTime.Now}");
            model.Messages.Add($"CurrentPrincipal-Name:{Thread.CurrentPrincipal.Identity.Name}  : {DateTime.Now}");

            CustomPrincipal customPrincipal = User as CustomPrincipal;
            if (customPrincipal != null)
            {
                model.Messages.Add($"MyFormsIdentity:{customPrincipal.Identity.Name}  : {customPrincipal.TimeZoneHour}");
                model.UserDataTimezoneHour = customPrincipal.TimeZoneHour;

            }
            model.UserNo = User.Identity?.Name;
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

        //https://stackoverrun.com/cn/q/5373741
        public ActionResult Login(string userNo, int userDataTimezoneHour = 8)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            var ticket = new FormsAuthenticationTicket(1, userNo, now, now.Add(FormsAuthentication.Timeout), true, $"1000;SecurityCode;{userDataTimezoneHour}", FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
                cookie.Expires = ticket.Expiration;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
                cookie.Domain = FormsAuthentication.CookieDomain;
            base.Response.Cookies.Add(cookie);
            return Redirect("Culture");
        }

        public ActionResult LoginThread()
        {
            FormsAuthentication.SignOut();

            return Redirect("Culture");

        }
    }
}