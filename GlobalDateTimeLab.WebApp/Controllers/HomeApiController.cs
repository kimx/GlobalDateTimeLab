using GlobalDateTimeLab.Console.Lib;
using GlobalDateTimeLab.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace GlobalDateTimeLab.WebApp.Controllers
{
    [RoutePrefix("api/HomeApi")]
    public class HomeApiController : ApiController
    {
        [HttpGet]
        [Route("GetTest")]
        public IEnumerable<string> GetTest()
        {
            var currentCultureDateTime = DateTimeExtensions.GetCustomCultureDateTime();
            var currency = 987654321.1234.ToString(string.Format("C2"));
            return new string[] { currentCultureDateTime.ToString(), currency };
        }

       

        [HttpPost]
        [Route("PostModel")]
        public HomeModel PostModel([FromBody]HomeModel model)
        {
            model.CreateDateTime = DateTimeExtensions.GetTestTaiwanDateTime();
            model.CreateUtcDateTime = DateTimeExtensions.GetTestUtcDateTime();
            if (model.UseUnSpecified)
            {
                model.CreateDateTime = DateTime.SpecifyKind(model.CreateDateTime, DateTimeKind.Unspecified);
                model.CreateUtcDateTime = DateTime.SpecifyKind(model.CreateUtcDateTime, DateTimeKind.Unspecified);
            }
            return model;
        }
    }
}