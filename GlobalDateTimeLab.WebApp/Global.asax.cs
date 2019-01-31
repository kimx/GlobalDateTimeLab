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
            //    var culture = new CustomCultureInfo("en", 2);

            HttpCookie timeZoneHourCookie = System.Web.HttpContext.Current.Request.Cookies["timezoneHour"];
            if (timeZoneHourCookie != null)
            {
                int timeZoneHour = Convert.ToInt32(timeZoneHourCookie.Value);
                var culture = CustomCultureInfo.Create(Thread.CurrentThread.CurrentCulture.Name, timeZoneHour);
                culture.NumberFormat.CurrencySymbol = "R";
                System.Web.HttpContext.Current.Items.Add("culture-begin", $"Global:{culture.UtcHours}:{DateTime.Now}");
                Thread.CurrentThread.CurrentCulture = culture;

                if (Request.IsAuthenticated)
                {
                    //FormsIdentity id = (FormsIdentity)User.Identity;
                    //MyFormsIdentity myFormsIdentity = new MyFormsIdentity(id.Ticket, timeZoneHour);
                    Context.User = new CustomPrincipal(User.Identity, timeZoneHour);

                    //https://blog.csdn.net/anihasiyou/article/details/79668267
                    //https://blog.csdn.net/lglgsy456/article/details/20616489
                    //若WinFomr or Console程式有用到時使用者資訊,就要設定如下:
                    //而取值則先判斷HttpContext.Current null再用Thread.CurrentPrincipal 
                    //這樣就可以作到Web及Win共用
                    // Thread.CurrentPrincipal =  new CustomPrincipal(User.Identity, timeZoneHour);


                }


                if (timeZoneHourCookie.Value == "2")
                    System.Threading.Thread.Sleep(10000);
                System.Web.HttpContext.Current.Items.Add("culture-end", $"Global:{culture.UtcHours}:{DateTime.Now}");

            }


        }
    }
}
