using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI.Utilities.Date
{
    public static class TimeFormat
    {
        public static bool IsTime(this string time )
        {
            TimeSpan timeSpan;
            return TimeSpan.TryParse(time, out timeSpan);
        }
        public static string GetStringTime() =>
            $"{((DateTime.Now.Hour >= 10 ? DateTime.Now.Hour : "0" +DateTime.Now.Hour))}:" +
            $"{((DateTime.Now.Minute >= 10 ? DateTime.Now.Minute : "0" + DateTime.Now.Minute))}:" +
            $"{((DateTime.Now.Second >= 10 ? DateTime.Now.Second : "0" + DateTime.Now.Second))}";
    }
}
