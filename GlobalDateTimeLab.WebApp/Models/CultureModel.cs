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

        public string UserNo { get; set; }

        public int TimezoneHour { get; set; }

        public int UserDataTimezoneHour { get; set; }


        public DateTime PricipleDateTime { get; set; }

        public DateTime CurrentCultureDateTime { get; set; }

        public List<string> Messages { get; set; }
    }
}