using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalDateTimeLab.Console.Lib
{
    public class JsonHelper
    {
        public static string ObjectToString<T>(T value, bool useUnSpecified = false)
        {
            //濾掉null http://www.newtonsoft.com/json/help/html/NullValueHandlingIgnore.htm
            //1.JsonConvert Include JsonSerializer的使用
            //https://json.codeplex.com/discussions/79713

            using (StringWriter sw = new StringWriter())
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;//2015/05/04
                    serializer.Formatting = Formatting.Indented;
                    if (useUnSpecified)
                        serializer.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                    serializer.Serialize(writer, value);
                }
                return sw.ToString();
            }
        }

        public static T StringToObject<T>(string json, DefaultContractResolver contractResolver)
        {
            using (StringReader sr = new StringReader(json))
            {
                using (JsonTextReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serializer.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
                    if (contractResolver != null)
                        serializer.ContractResolver = contractResolver;
                    return serializer.Deserialize<T>(reader);
                }
            }
        }
    }
}
