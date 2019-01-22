using GlobalDateTimeLab.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GlobalDateTimeLab.WebApp.Controllers
{
    [RoutePrefix("api/HomeApi")]
    public class HomeApiController : ApiController
    {
        [HttpGet]
        [Route("GetTest")]
        public IEnumerable<string> GetTest()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("PostModel")]
        public HomeModel PostModel([FromBody]HomeModel model)
        {
            model.CreateDateTime = DateTime.UtcNow.AddHours(8);
            model.CreateUtcDateTime = DateTime.UtcNow;
            if (model.UseUnSpecified)
            {
                model.CreateDateTime = DateTime.SpecifyKind(model.CreateDateTime, DateTimeKind.Unspecified);
                model.CreateUtcDateTime = DateTime.SpecifyKind(model.CreateUtcDateTime, DateTimeKind.Unspecified);
            }
            return model;
        }
    }
}