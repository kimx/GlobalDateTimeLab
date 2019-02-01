using GlobalDateTimeLab.Console.Lib;
using GlobalDateTimeLab.WebApp.App_Start;
using GlobalDateTimeLab.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

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
            int timeZoneHour = 8;
            //CurrentCulture Version
            HttpCookie timeZoneHourCookie = System.Web.HttpContext.Current.Request.Cookies["timezoneHour"];
            if (timeZoneHourCookie != null)
            {
                timeZoneHour = Convert.ToInt32(timeZoneHourCookie.Value);
                var culture = CustomCultureInfo.Create(Thread.CurrentThread.CurrentCulture.Name, timeZoneHour);
                culture.NumberFormat.CurrencySymbol = "R";
                System.Web.HttpContext.Current.Items.Add("culture-begin", $"Global:{culture.UtcHours}:{DateTime.Now}");
                Thread.CurrentThread.CurrentCulture = culture;

                //if (timeZoneHourCookie.Value == "2")
                //    System.Threading.Thread.Sleep(10000);
                System.Web.HttpContext.Current.Items.Add("culture-end", $"Global:{culture.UtcHours}:{DateTime.Now}");

            }

            //CustomPrincipal Version 20190209 參考此版使用在專案上
            if (Request.IsAuthenticated)
            {
                FormsIdentity id = (FormsIdentity)User.Identity;
                Context.User = new CustomPrincipal(User.Identity, id.Ticket.UserData);
            }
            //else
            //{
            //    //兩者都要指定,CurrentPrincipal才有作用
            //    Thread.CurrentPrincipal = new CustomPrincipal(new GenericIdentity("kimxinfo-without-formidentity"), 0);
            //    Context.User = Thread.CurrentPrincipal;
            //}



        }


    }




}
