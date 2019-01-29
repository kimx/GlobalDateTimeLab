using GlobalDateTimeLab.Console.Lib;
using GlobalDateTimeLab.WebApp.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GlobalDateTimeLab.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);//順序1
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //    var culture = new CustomCultureInfo("en", 2);

            HttpCookie timezoneHourCookie = System.Web.HttpContext.Current.Request.Cookies["timezoneHour"];
            if (timezoneHourCookie != null)
            {
                var culture = new CustomCultureInfo(Thread.CurrentThread.CurrentCulture.Name, Convert.ToInt32(timezoneHourCookie.Value));
                culture.NumberFormat.CurrencySymbol = "R";
                Thread.CurrentThread.CurrentCulture = culture;
            }


        }
    }
}
