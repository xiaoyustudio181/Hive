using System;
using System.Collections.Generic;
using System.Text;

namespace MyFrame
{
    public class MyTime
    {
        DateTime Original;
        long OneDayTicks = 864000000000;//1天的Ticks
        long OneHourTicks = 36000000000;//1小时的Ticks
        long OneMinuteTicks = 600000000;//1分钟的Ticks
        long OneSecondTicks = 10000000;//1秒钟的Ticks
        long OneMilliSecondTicks = 10000;//1毫秒的Ticks
        public MyTime(DateTime Original)
        {
            this.Original = Original;
        }
        #region 时间加减（DateTime对象自带更强大的AddXXX(double)功能）
        /// <summary>
        /// 获取从指定时间起按天数变化后的日期对象。
        /// </summary>
        /// <param name="n">变化的天数（可负）。</param>
        /// <returns></returns>
        public DateTime DayVary(int n)
        {
            return new DateTime(Original.Ticks + n * OneDayTicks);
        }
        /// <summary>
        /// 获取从指定时间起按小时数变化后的日期对象。
        /// </summary>
        /// <param name="n">变化的小时数（可负）。</param>
        /// <returns></returns>
        public DateTime HourVary(int n)
        {
            return new DateTime(Original.Ticks + n * OneHourTicks);
        }
        /// <summary>
        /// 获取从指定时间起按分钟数变化后的日期对象。
        /// </summary>
        /// <param name="n">变化的分钟数（可负）。</param>
        /// <returns></returns>
        public DateTime MinuteVary(int n)
        {
            return new DateTime(Original.Ticks + n * OneMinuteTicks);
        }
        /// <summary>
        /// 获取从指定时间起按秒钟数变化后的日期对象。
        /// </summary>
        /// <param name="n">变化的秒钟数（可负）。</param>
        /// <returns></returns>
        public DateTime SecondVary(int n)
        {
            return new DateTime(Original.Ticks + n * OneSecondTicks);
        }
        /// <summary>
        /// 获取从指定时间起按毫秒数变化后的日期对象。
        /// </summary>
        /// <param name="n">变化的毫秒数（可负）。</param>
        /// <returns></returns>
        public DateTime MilliSecondVary(int n)
        {
            return new DateTime(Original.Ticks + n * OneMilliSecondTicks);
        }
        #endregion
        static DateTime now;//变化的时间
        static DateTime Now;//不变的时间
        public static string GetNowTimeCode(int TypeID=0)
        {
            now = DateTime.Now;
            Now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
            switch (TypeID)
            {
                case 1: return Now.ToString("yyyy-MM-dd HH:mm:ss");
                case 2: return Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                case 3: return Now.ToString("yyyyMMdd.HHmmss");
                case 4: return Now.ToString("yyyyMMdd.HHmmss.fff");
                default: return Now.ToString("yyMMdd.HHmmss.fff");
            }
        }
        static string temp;
        public static string GetNowDateTimeInChinese()
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday: temp = "星期一"; break;
                case DayOfWeek.Tuesday: temp = "星期二"; break;
                case DayOfWeek.Wednesday: temp = "星期三"; break;
                case DayOfWeek.Thursday: temp = "星期四"; break;
                case DayOfWeek.Friday: temp = "星期五"; break;
                case DayOfWeek.Saturday: temp = "星期六"; break;
                case DayOfWeek.Sunday: temp = "星期天"; break;
                default: temp = ""; break;
            }
            return DateTime.Now.ToString("yyyy年MM月dd日  " + temp + "  HH:mm:ss");
        }
    }
}
