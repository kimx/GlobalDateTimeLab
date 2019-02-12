using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace GlobalDateTimeLab.Console.Lib
{
    public class CustomPrincipal : GenericPrincipal
    {
        public string CompanyNo { get; set; }
        public string SecurityStamp { get; set; }
        public int TimeZoneHour { get; set; }


        public CustomPrincipal(IIdentity identity, string userData) : base(identity, null)
        {
            var userDataArray = userData.Split(';');
            if (userDataArray.Length >= 1)
                CompanyNo = userDataArray[0];
            if (userDataArray.Length >= 2)
                SecurityStamp = userDataArray[1];
            if (userDataArray.Length >= 3)
                TimeZoneHour = Convert.ToInt32(userDataArray[2]);
        }



    }
}
