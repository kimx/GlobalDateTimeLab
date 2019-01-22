using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace GlobalDateTimeLab.WebApp.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
name: "AreaAPI",
routeTemplate: "api/{area}/{controller}/{id}",
defaults: new { id = RouteParameter.Optional });


            RegisterApiFormatter(config);
        }

        private static void RegisterApiFormatter(HttpConfiguration config)
        {
            //http://blog.miniasp.com/post/2012/10/13/ASPNET-Web-API-Force-return-JSON-format-instead-of-XML-for-Google-Chrome-Firefox-Safari.aspx
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            var contractResolver = (DefaultContractResolver)config.Formatters.JsonFormatter.SerializerSettings.ContractResolver;
            contractResolver.IgnoreSerializableAttribute = true;
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
           //送上來的不處理Kind要保留原值，若要處理的話是時間的增加及UTC要處理，因為與MVC的行為不冋
            // config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Unspecified;
        }
    }
}