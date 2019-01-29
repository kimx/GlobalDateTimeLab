using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalDateTimeLab.Console.Lib
{
    public class CustomCultureInfo : CultureInfo
    {
        public int UtcHours { get; set; }
        public CustomCultureInfo(string name, int utcHours) : base(name)
        {
            this.UtcHours = utcHours;
        }
    }
}
