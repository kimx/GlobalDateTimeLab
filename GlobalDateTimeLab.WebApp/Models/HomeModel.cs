using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalDateTimeLab.WebApp.Models
{
    public class HomeModel
    {
        public DateTime CreateDate { get; set; }

        public DateTime CreateDateTime { get; set; }
        public DateTime CreateUtcDateTime { get; set; }

        /// <summary>
        /// 是否使用Json.Net的全域序列化
        /// </summary>
        public bool UseUnSpecified { get; set; }
    }
}