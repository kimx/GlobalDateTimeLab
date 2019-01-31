using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalDateTimeLab.WebApp.Models
{
    public class CultureModel
    {
        public CultureModel()
        {
            Messages = new List<string>();
        }

        public int TimezoneHour { get; set; }

        public DateTime CurrentCultureDateTime { get; set; }

        public List<string> Messages { get; set; }
    }
}