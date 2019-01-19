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

namespace GlobalDateTimeLab.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            預設值測試();
            //資料庫測試();
            System.Console.Read();
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

    }
}
