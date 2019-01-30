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
        public static Dictionary<string, CustomCultureInfo> _dict = new Dictionary<string, CustomCultureInfo>();
        private static readonly object _lockObject = new object();
        public static CustomCultureInfo Create(string name, int utcHours)
        {
            string key = $"{name}_{utcHours}";
            if (_dict.ContainsKey(key))
                return _dict[key];
            lock (_lockObject)
            {
                if (_dict.ContainsKey(key))
                    return _dict[key];
                var culture = new CustomCultureInfo(name, utcHours);
                _dict.Add(key, culture);
                return culture;
            }
        }

        public int UtcHours { get; set; }
        private CustomCultureInfo(string name, int utcHours) : base(name)
        {
            this.UtcHours = utcHours;
        }
    }
}
