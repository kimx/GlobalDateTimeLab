using GlobalDateTimeLab.Console.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GlobalDateTimeLab.WebApp.Models
{
    public class ScJsonResult : JsonResult
    {
        //public string Callback
        //{
        //    get;
        //    set;
        //}
        public object JData
        {
            get;
            set;
        }


        bool UseUnSpecified;
        public ScJsonResult(object data, bool useUnSpecified)
        {
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            JData = data;
            UseUnSpecified = useUnSpecified;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            //var dataToSend = JsonConvert.SerializeObject(JData);
            var dataToSend = JsonHelper.ObjectToString(JData, UseUnSpecified);
            var ctx = context.HttpContext;
            context.HttpContext.Response.ContentType = "application/json";

            ctx.Response.Write(dataToSend);
        }
    }
}