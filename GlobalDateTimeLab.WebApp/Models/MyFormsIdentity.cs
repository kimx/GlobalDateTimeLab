using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace GlobalDateTimeLab.WebApp.Models
{
    public class MyFormsIdentity : FormsIdentity
    {
        public int TimeZoneHour { get; set; }

        public MyFormsIdentity(FormsAuthenticationTicket ticket, int timeZoneHour) : base(ticket)
        {
            this.TimeZoneHour = timeZoneHour;
        }
    }



}