using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalDateTimeLab.WebApp.Models
{
    public class CultureModel
    {
        public int TimezoneHour { get; set; }

        public DateTime CurrentCultureDateTime { get; set; }
    }
}