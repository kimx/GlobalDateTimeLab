using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalDateTimeLab.Console.Lib;
using GlobalDateTimeLab.Console.Entites;
using GlobalDateTimeLab.Console.Models;
using System.Threading;
using System.Globalization;
using System.Security.Principal;

namespace GlobalDateTimeLab.Console
{
    class Program
    {
        static Dictionary<string, string> _timeZoneHours = new Dictionary<string, string>();
        static void Main(string[] args)
        {
            _timeZoneHours.Add("UTC", "0");
            _timeZoneHours.Add("South Africa", "2");
            _timeZoneHours.Add("Dubai", "4");
            _timeZoneHours.Add("Taiwan", "8");

            ToUserTimeLab();
            //資料庫測試();
            System.Console.Read();
        }

        static void ToUserTimeLab()
        {
            foreach (var info in TimeZoneInfo.GetSystemTimeZones())
            {
                if (info.Id=="Arabian Standard Time")
                    System.Console.WriteLine(info.Id);
            }
            DateTime utc = DateTime.UtcNow;
            DateTime za = utc.ToUserTime("South Africa Standard Time");
            DateTime dubai = utc.ToUserTime("Arabian Standard Time");
            DateTime taiwan = utc.ToUserTime("Taipei Standard Time");
            System.Console.WriteLine(utc);
            System.Console.WriteLine(za);
            System.Console.WriteLine(dubai);
            System.Console.WriteLine(taiwan);
        }

        static void CustomPricipleLab()
        {
            foreach (var timeZone in _timeZoneHours)
            {
                Thread.CurrentPrincipal = new CustomPrincipal(new GenericIdentity(timeZone.Key), $"{timeZone.Key};security;{timeZone.Value}");
                var priciple = DateTimeExtensions.GetThreadCustomPrincipal();
                System.Console.WriteLine($"CompanyNo : {priciple.CompanyNo} ,TimeZoneHour : {priciple.TimeZoneHour}");
                System.Console.WriteLine($"{DateTimeExtensions.GetUserThreadPricipleDateTime()}");

            }


            //Thread.CurrentPrincipal = new CustomPrincipal(new GenericIdentity("kimxinfo-console"), "1000;security;0");
            //var priciple = DateTimeExtensions.GetThreadCustomPrincipal();
            //System.Console.WriteLine($"UTC :            {DateTimeExtensions.GetUserThreadPricipleDateTime()} ,{priciple.TimeZoneHour}");

            //Thread.CurrentPrincipal = new CustomPrincipal(new GenericIdentity("kimxinfo-console"), "1000;security;2");
            //priciple = DateTimeExtensions.GetThreadCustomPrincipal();
            //System.Console.WriteLine($"South Africa :   {DateTimeExtensions.GetUserThreadPricipleDateTime()} ,{priciple.TimeZoneHour}");

            //Thread.CurrentPrincipal = new CustomPrincipal(new GenericIdentity("kimxinfo-console"), "1000;security;4");
            //priciple = DateTimeExtensions.GetThreadCustomPrincipal();
            //System.Console.WriteLine($"Dubai :          {DateTimeExtensions.GetUserThreadPricipleDateTime()} ,{priciple.TimeZoneHour}");

            //Thread.CurrentPrincipal = new CustomPrincipal(new GenericIdentity("kimxinfo-console"), "1000;security;8");
            //priciple = DateTimeExtensions.GetThreadCustomPrincipal();
            //System.Console.WriteLine($"Taiwan :         {DateTimeExtensions.GetUserThreadPricipleDateTime()} ,{priciple.TimeZoneHour}");
        }


        static void CustomCurrentCultureLab()
        {
            Thread.CurrentThread.CurrentCulture = CustomCultureInfo.Create(Thread.CurrentThread.CurrentCulture.Name, 0);
            System.Console.WriteLine($"UTC :          {DateTimeExtensions.GetCustomCultureDateTime()}");

            Thread.CurrentThread.CurrentCulture = CustomCultureInfo.Create(Thread.CurrentThread.CurrentCulture.Name, 2);
            System.Console.WriteLine($"South Africa : {DateTimeExtensions.GetCustomCultureDateTime()}");

            Thread.CurrentThread.CurrentCulture = CustomCultureInfo.Create(Thread.CurrentThread.CurrentCulture.Name, 4);
            System.Console.WriteLine($"Dubai :        {DateTimeExtensions.GetCustomCultureDateTime()}");

            Thread.CurrentThread.CurrentCulture = CustomCultureInfo.Create(Thread.CurrentThread.CurrentCulture.Name, 8);
            System.Console.WriteLine($"Taiwan :       {DateTimeExtensions.GetCustomCultureDateTime()}");

            ShowCultureFormat(new CultureInfo("en"));
            ShowCultureFormat(new CultureInfo("en-US"));
            ShowCultureFormat(new CultureInfo("en-ZA"));
            ShowCultureFormat(new CultureInfo("ar-AE"));//Arabic (United Arab Emirates) (ar-AE) - 杜拜
            //foreach (var item in CultureInfo.GetCultures(CultureTypes.SpecificCultures).Where(o => o.Name.StartsWith("ar-")))
            //{
            //    System.Console.WriteLine($"{item.Name} : {item.DisplayName} : {item.EnglishName}");
            //}
        }

        private static void ShowCultureFormat(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Console.WriteLine($"{Thread.CurrentThread.CurrentCulture.Name} : {Thread.CurrentThread.CurrentCulture.DisplayName} : {Thread.CurrentThread.CurrentCulture.EnglishName}");
            System.Console.WriteLine($"{DateTimeExtensions.GetCustomCultureDateTime()}");
            System.Console.WriteLine(1234567.5432.ToString("N2"));
            System.Console.WriteLine(1234567.5432.ToString("C2"));
            System.Console.WriteLine();
        }




        static void 預設值測試()
        {
            TestDto before = new TestDto();
            before.UtcDateTime = DateTimeExtensions.GetTestUtcDateTime();
            before.TaiwanDateTime = DateTimeExtensions.GetTestTaiwanDateTime();
            string json = JsonHelper.ObjectToString(before);
            System.Console.WriteLine(json);

            TestDto after = JsonHelper.StringToObject<TestDto>(json, null);
            System.Console.WriteLine($"UtcDateTime : {after.UtcDateTime},{after.UtcDateTime.Kind}");
            System.Console.WriteLine($"TaiwanDateTime : {after.TaiwanDateTime},{after.TaiwanDateTime.Kind}");
        }

        static void 資料庫測試()
        {
            using (var db = new Entites.TestEntities())
            {
                db.Database.ExecuteSqlCommand("delete from dbo.DateTimeTable");

                DateTimeTable utcEntity = new DateTimeTable();
                utcEntity.Name = "utcEntity";
                utcEntity.CreateTime = DateTimeExtensions.GetTestUtcDateTime();
                db.DateTimeTables.Add(utcEntity);

                DateTimeTable taiwanEntity = new DateTimeTable();
                taiwanEntity.CreateTime = DateTimeExtensions.GetTestTaiwanDateTime();
                taiwanEntity.Name = "taiwanEntity";
                db.DateTimeTables.Add(taiwanEntity);
                db.SaveChanges();
                System.Console.WriteLine("資料庫測試");
                System.Console.WriteLine($"utcEntity : {utcEntity.CreateTime},{utcEntity.CreateTime?.Kind}");
                System.Console.WriteLine($"taiwanEntity : {taiwanEntity.CreateTime},{taiwanEntity.CreateTime?.Kind}");

                System.Console.WriteLine();


            }
            System.Console.WriteLine();
            using (var db = new Entites.TestEntities())
            {

                DateTimeTable utcEntity = db.DateTimeTables.SingleOrDefault(o => o.Name == "utcEntity");
                DateTimeTable taiwanEntity = db.DateTimeTables.SingleOrDefault(o => o.Name == "taiwanEntity");

                System.Console.WriteLine("資料庫測試-從資料庫讀取");
                System.Console.WriteLine($"utcEntity : {utcEntity.CreateTime},{utcEntity.CreateTime?.Kind}");
                System.Console.WriteLine($"taiwanEntity : {taiwanEntity.CreateTime},{taiwanEntity.CreateTime?.Kind}");
                System.Console.WriteLine();

                System.Console.WriteLine("資料庫測試-從資料庫讀取-序列化");
                string utcJson = JsonHelper.ObjectToString(utcEntity);
                string taiwanJson = JsonHelper.ObjectToString(taiwanEntity);
                System.Console.WriteLine("utcEntity:" + utcJson);
                System.Console.WriteLine("taiwanEntity:" + taiwanJson);

                System.Console.WriteLine("資料庫測試-從資料庫讀取-反序列化");
                utcEntity = JsonHelper.StringToObject<DateTimeTable>(utcJson, null);
                taiwanEntity = JsonHelper.StringToObject<DateTimeTable>(taiwanJson, null);

                System.Console.WriteLine($"utcEntity : {utcEntity.CreateTime},{utcEntity.CreateTime?.Kind}");
                System.Console.WriteLine($"taiwanEntity : {taiwanEntity.CreateTime},{taiwanEntity.CreateTime?.Kind}");

            }
        }

        private static Object rndLock = new Object();
        static void CustomCultureMultiThreadRandowLab()
        {
            System.Console.WriteLine("CustomCultureMultiThreadRandowLab-Start");
            Random rnd = new Random();
            var tasks = new List<Task<int>>();
            for (int ctr = 1; ctr <= 20; ctr++)
            {
                tasks.Add(Task.Factory.StartNew(
                 () =>
                 {
                     int s = 0;
                     for (int n = 0; n <= 999; n++)
                     {
                         lock (rndLock)
                         {
                             s = rnd.Next(24);
                             System.Threading.Thread.Sleep(1 * s);
                             CustomCultureInfo.Create("en", s);
                         }
                     }
                     return CustomCultureInfo._dict.Count;
                 }));
            }
            Task.WaitAll(tasks.ToArray());
            System.Console.WriteLine(CustomCultureInfo._dict.Count);
            System.Console.WriteLine("CustomCultureMultiThreadRandowLab-End");
        }

    }
}
