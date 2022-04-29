/***************************
 * Author:      luwenzhen
 * CreateTime:  9/28/2017
 * LastEditor:
 * Description:
 * 
**/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeTool
{

    public const uint DAY_HOUR = 24;            // 一天的小时数
    public const uint DAY_MINUTE = 1440;        // 一天的分钟数
    public const uint DAY_SECOND = 86400;       // 一天的秒数
    public const uint HOUR_MINUTE = 60;         // 一小时的分钟数
    public const uint HOUR_SECOND = 3600;       // 一小时的秒数
    public const uint MINUTE_SECOND = 60;       // 一分钟的秒数
    public const uint SECOND_MILLISEC = 1000;   // 1秒包含的毫秒数
    public const long SECOND_NS = 1000000000;   // 1秒对应的纳秒
    public const uint MILLISEC_NS = 1000000;    // 1毫秒对应的纳秒
    public const ulong MILLISEC_1900_1970 = 2208988800000; // 1900-01-01 ~ 1970-01-01相差的时间（毫秒）
    public static readonly string[] NTP_SERVERS = { "time.windows.com", "time.apple.com", "time.google.com", "ntp.aliyun.com" };

    public static string FormatSecondToDHM(uint second, string timeText = "{0}:{1}:{2}")
    {
        int day = Mathf.FloorToInt((float)second / DAY_SECOND);
        string dayDesc = string.Format("{0:D2}", day);
        second = second % DAY_SECOND;
        int hour = Mathf.FloorToInt((float)second / HOUR_SECOND);
        string hourDesc = string.Format("{0:D2}", hour);
        second = second % HOUR_SECOND;
        int min = Mathf.FloorToInt((float)second / MINUTE_SECOND);
        string minuteDesc = string.Format("{0:D2}", min);

        return string.Format(timeText, dayDesc, hourDesc, minuteDesc);
    }

    public static string FormatSecondToDHMS(uint second, string timeText = "{0} days {1}:{2}:{3}")
    {
        int day = Mathf.FloorToInt((float)second / DAY_SECOND);
        string dayDesc = string.Format("{0}", day);
        second = second % DAY_SECOND;
        int hour = Mathf.FloorToInt((float)second / HOUR_SECOND);
        string hourDesc = string.Format("{0:D2}", hour);
        second = second % HOUR_SECOND;
        int min = Mathf.FloorToInt((float)second / MINUTE_SECOND);
        string minuteDesc = string.Format("{0:D2}", min);
        second = second % MINUTE_SECOND;
        string secondDesc = string.Format("{0:D2}", second);

        return string.Format(timeText, dayDesc, hourDesc, minuteDesc, secondDesc);
    }

    public static string FormatSecondToHM(uint second, string timeText = "{0}:{1}")
    {
        int hour = Mathf.FloorToInt((float)second / HOUR_SECOND);
        string hourDesc = string.Format("{0:D2}", hour);
        second = second % HOUR_SECOND;
        int min = Mathf.FloorToInt((float)second / MINUTE_SECOND);
        string minuteDesc = string.Format("{0:D2}", min);

        return string.Format(timeText, hourDesc, minuteDesc);
    }

    public static string FormatSecondToHMS(uint second, string timeText = "{0}:{1}:{2}")
    {
        int hour = Mathf.FloorToInt((float)second / HOUR_SECOND);
        string hourDesc = string.Format("{0:D2}", hour);
        second = second % HOUR_SECOND;
        int min = Mathf.FloorToInt((float)second / MINUTE_SECOND);
        string minuteDesc = string.Format("{0:D2}", min);
        second = second % MINUTE_SECOND;
        string secondDesc = string.Format("{0:D2}", second);

        return string.Format(timeText, hourDesc, minuteDesc, secondDesc);
    }

    /// <summary>
    /// 格式化秒变成 00:00的格式, 或者自定义文本
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string FormatSecondToMMSS(uint second, string timeText = "{0}:{1}")
    {
        uint minute = second / MINUTE_SECOND;
        uint leftSecond = second % MINUTE_SECOND;
        string minuteDesc = string.Format("{0:D2}", minute);
        string secondDesc = string.Format("{0:D2}", leftSecond);
        return string.Format(timeText, minuteDesc, secondDesc);
    }

    /// <summary>
    /// 秒转天 或 时分秒
    /// </summary>
    /// <param name="second"></param>
    /// <param name="dayText"></param>
    /// <param name="timeText"></param>
    /// <returns></returns>
    public static string FormatSecondToBestFit(uint second, string dayText = "{0} days {1} hour", string timeText = "{0}:{1}:{2}")
    {
        if (second >= DAY_SECOND)
        {
            uint day = second / DAY_SECOND;
            second %= DAY_SECOND;
            uint hour = second / HOUR_SECOND;
            return string.Format(dayText, day, hour);
        }
        else
        {
            return FormatSecondToHMS(second, timeText);
        }
    }

    /// <summary>
    /// 秒转天 或 时分秒
    /// </summary>
    /// <param name="second"></param>
    /// <param name="dayText"></param>
    /// <param name="timeText"></param>
    /// <returns></returns>
    public static string FormatSecondToFitUnit(uint second, string dayText = "{0} days", string hourText = "{0} hours", string minText = "{0} mins", string secText = "{0}s")
    {
        int cnt;
        if (second >= DAY_SECOND)
        {
            cnt = Mathf.FloorToInt((float)second / DAY_SECOND);
            return string.Format(dayText, cnt);
        }
        else if (second >= HOUR_SECOND)
        {
            cnt = Mathf.FloorToInt((float)second / HOUR_SECOND);
            return string.Format(hourText, cnt);
        }
        else if (second >= MINUTE_SECOND)
        {
            cnt = Mathf.FloorToInt((float)second / MINUTE_SECOND);
            return string.Format(minText, cnt);
        }
        else
        {
            return string.Format(secText, second);
        }
    }

    /// <summary>
    /// 格式化日期, {d}对应天,{h}对应小时...
    /// </summary>
    /// <param name="second">秒</param>
    /// <param name="formatter">格式化参数</param>
    /// <returns></returns>
    public static string FormatSecond(uint second, string formatter = "{d}d{h}h{m}m{s}s")
    {
        uint day = second / DAY_SECOND;
        uint hour = (second % DAY_SECOND) / HOUR_SECOND;
        uint min = (second % HOUR_SECOND) / MINUTE_SECOND;
        uint sec = (second % MINUTE_SECOND);
        formatter = formatter.Replace("{d}", string.Format("{0:D2}", day));
        formatter = formatter.Replace("{h}", string.Format("{0:D2}", hour));
        formatter = formatter.Replace("{m}", string.Format("{0:D2}", min));
        formatter = formatter.Replace("{s}", string.Format("{0:D2}", sec));
        return formatter;
    }

    /// <summary>
    /// 通过utc秒获得
    /// </summary>
    /// <param name="utcSeconds"></param>
    /// <returns></returns>
    public static DateTime GetDatetimeFromUtcsecond(double utcSeconds)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        startTime = startTime.AddSeconds(utcSeconds);
        startTime.Add(TimeZoneInfo.Utc.BaseUtcOffset);   //转化为当地时区时间
        return startTime;
    }

    /// <summary>
    /// utc秒转换成 MM/dd/yy HH:mm:ss的格式
    /// </summary>
    /// <param name="utcSecond"></param>
    /// <returns></returns>
    public static string UtcsecondToMDYHMS(uint utcSecond, string format = "MM/dd/yy HH:mm:ss")
    {
        DateTime dateTime = GetDatetimeFromUtcsecond(utcSecond);
        return dateTime.ToString(format);
    }

    /// 获取当前时间戳 
    /// </summary>
    /// <returns></returns>
    public static uint GetUtcSecond()
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
        double timeStamp = (DateTime.Now - startTime).TotalSeconds;         // 相差秒数

        return (uint)timeStamp;
    }

    /// 获取当前时间戳(毫秒)
    /// </summary>
    /// <returns></returns>
    public static long GetUtcMilliSecond()
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
        long timeStamp = (DateTime.Now - startTime).Ticks / (MILLISEC_NS / 100);

        return timeStamp;
    }

    /// <summary>
    /// 获取时间对应的时间戳 
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static uint GetUtcSecond(DateTime dateTime)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
        double timeStamp = (dateTime - startTime).TotalSeconds;         // 相差秒数

        return (uint)timeStamp;
    }

    /// <summary>
    /// 获取指定utc时间戳的本地时区当日0时时间戳
    /// timestamp为0取当前时间戳
    /// </summary>
    /// <returns></returns>
    public static uint GetDayBegin(uint timestamp)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
        DateTime dateTime = 0 == timestamp ? DateTime.Now : GetDatetimeFromUtcsecond(timestamp);
        dateTime = dateTime.AddHours(-dateTime.Hour);
        dateTime = dateTime.AddMinutes(-dateTime.Minute);
        dateTime = dateTime.AddSeconds(-dateTime.Second);
        dateTime = dateTime.AddMilliseconds(-dateTime.Millisecond);
        double timeStamp = (dateTime - startTime).TotalSeconds;         // 相差秒数
        return (uint)timeStamp;
    }

    /// <summary>
    /// 是否是同一天
    /// </summary>
    /// <param name="dt1"></param>
    /// <param name="dt2"></param>
    /// <returns></returns>
    public static bool IsSameDay(DateTime dt1, DateTime dt2)
    {
        if (dt1.Year == dt2.Year && dt1.DayOfYear == dt2.DayOfYear)
            return true;
        return false;
    }

    /// <summary>
    /// 是否是同一周
    /// </summary>
    /// <param name="dt1"></param>
    /// <param name="dt2"></param>
    /// <returns></returns>
    public static bool IsSameWeek(DateTime dt1, DateTime dt2)
    {
        if (dt1 > dt2)
        {
            DateTime temp = dt1;
            dt1 = dt2;
            dt2 = temp;
        }
        double daySpan = (dt2.Date - dt1.Date).TotalDays;
        if (daySpan >= 7)
            return false;
        int intDayOfWeek = (int)dt2.DayOfWeek;
        if (intDayOfWeek == 0)
            intDayOfWeek = 7;
        if (daySpan >= intDayOfWeek)
            return false;
        return true;
    }

    /// <summary>
    /// 是否是同一个月
    /// </summary>
    /// <param name="dt1"></param>
    /// <param name="dt2"></param>
    /// <returns></returns>
    public static bool IsSameMonth(DateTime dt1, DateTime dt2)
    {
        return dt1.Year == dt2.Year && dt1.Month == dt2.Month;
    }

    /// <summary>
    /// 获取两个时间相差的天数
    /// </summary>
    /// <param name="dt1"></param>
    /// <param name="dt2"></param>
    /// <returns></returns>
    public static int GetTimeSpanDays(DateTime dt1, DateTime dt2)
    {
        dt1 = new DateTime(dt1.Year, dt1.Month, dt1.Day, 0, 0, 0);
        dt2 = new DateTime(dt2.Year, dt2.Month, dt2.Day, 0, 0, 0);
        TimeSpan timeSpan = dt2.Subtract(dt1);
        return Mathf.Abs((int)timeSpan.TotalDays);
    }

    /// <summary>
    /// 获取一个月有几天
    /// </summary>
    /// <returns></returns>
    public static int GetMonthNumberDay(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }


}

public class TimeRecord
{
    public long timeStart;
    public long timeEnd;
    
    public void start()
    {
        timeStart = TimeTool.GetUtcMilliSecond();
    }

    public void end(string recordName = "")
    {
        timeEnd = TimeTool.GetUtcMilliSecond();

        Debug.Log(string.Format("{0}: {1} MS", recordName, timeEnd-timeStart));
    }
}

//本文较详细地介绍了C#的DateTime对象的使用方法.DateTime.Now.ToShortTimeString() 
//DateTime dt = DateTime.Now; 
//dt.ToString();//2005-11-5 13:21:25 
//dt.ToFileTime().ToString();//127756416859912816 
//dt.ToFileTimeUtc().ToString();//127756704859912816 
//dt.ToLocalTime().ToString();//2005-11-5 21:21:25 
//dt.ToLongDateString().ToString();//2005年11月5日 
//dt.ToLongTimeString().ToString();//13:21:25 
//dt.ToOADate().ToString();//38661.5565508218 
//dt.ToShortDateString().ToString();//2005-11-5 
//dt.ToShortTimeString().ToString();//13:21 
//dt.ToUniversalTime().ToString();//2005-11-5 5:21:25 
//dt.Year.ToString();//2005 
//dt.Date.ToString();//2005-11-5 0:00:00 
//dt.DayOfWeek.ToString();//Saturday 
//dt.DayOfYear.ToString();//309 
//dt.Hour.ToString();//13 
//dt.Millisecond.ToString();//441 
//dt.Minute.ToString();//30 
//dt.Month.ToString();//11 
//dt.Second.ToString();//28 
//dt.Ticks.ToString();//632667942284412864 
//dt.TimeOfDay.ToString();//13:30:28.4412864 
//dt.ToString();//2005-11-5 13:47:04 
//dt.AddYears(1).ToString();//2006-11-5 13:47:04 
//dt.AddDays(1.1).ToString();//2005-11-6 16:11:04 
//dt.AddHours(1.1).ToString();//2005-11-5 14:53:04 
//dt.AddMilliseconds(1.1).ToString();//2005-11-5 13:47:04 
//dt.AddMonths(1).ToString();//2005-12-5 13:47:04 
//dt.AddSeconds(1.1).ToString();//2005-11-5 13:47:05 
//dt.AddMinutes(1.1).ToString();//2005-11-5 13:48:10 
//dt.AddTicks(1000).ToString();//2005-11-5 13:47:04 
//dt.CompareTo(dt).ToString();//0 
//dt.Add(?).ToString();//问号为一个时间段 
//dt.Equals("2005-11-6 16:11:04").ToString();//False 
//dt.Equals(dt).ToString();//True 
//dt.GetHashCode().ToString();//1474088234 
//dt.GetType().ToString();//System.DateTime 
//dt.GetTypeCode().ToString();//DateTime
//dt.GetDateTimeFormats('s')[0].ToString();//2005-11-05T14:06:25 
//dt.GetDateTimeFormats('t')[0].ToString();//14:06 
//dt.GetDateTimeFormats('y')[0].ToString();//2005年11月 
//dt.GetDateTimeFormats('D')[0].ToString();//2005年11月5日 
//dt.GetDateTimeFormats('D')[1].ToString();//2005 11 05 
//dt.GetDateTimeFormats('D')[2].ToString();//星期六 2005 11 05 
//dt.GetDateTimeFormats('D')[3].ToString();//星期六 2005年11月5日 
//dt.GetDateTimeFormats('M')[0].ToString();//11月5日 
//dt.GetDateTimeFormats('f')[0].ToString();//2005年11月5日 14:06 
//dt.GetDateTimeFormats('g')[0].ToString();//2005-11-5 14:06 
//dt.GetDateTimeFormats('r')[0].ToString();//Sat, 05 Nov 2005 14:06:25 GMT 
//string.Format("{0:d}", dt);//2005-11-5 
//string.Format("{0}", dt);//2005年11月5日 
//string.Format("{0:f}", dt);//2005年11月5日 14:23 
//string.Format("{0:F}", dt);//2005年11月5日 14:23:23 
//string.Format("{0:g}", dt);//2005-11-5 14:23 
//string.Format("{0:G}", dt);//2005-11-5 14:23:23 
//string.Format("{0:M}", dt);//11月5日 
//string.Format("{0:R}", dt);//Sat, 05 Nov 2005 14:23:23 GMT 
//string.Format("{0:s}", dt);//2005-11-05T14:23:23 
//string.Format("{0:t}", dt);//14:23 
//string.Format("{0:T}", dt);//14:23:23 
//string.Format("{0:u}", dt);//2005-11-05 14:23:23Z 
//string.Format("{0:U}", dt);//2005年11月5日 6:23:23 
//string.Format("{0:Y}", dt);//2005年11月 
//string.Format("{0}", dt);//2005-11-5 14:23:23 
//string.Format("{0:yyyyMMddHHmmssffff}", dt);
//计算2个日期之间的天数差 
//----------------------------------------------- 
//DateTime dt1 = Convert.DateTime("2007-8-1");
//DateTime dt2 = Convert.DateTime("2007-8-15");
//TimeSpan span = dt2.Subtract(dt1);
//int dayDiff = span.Days + 1;
//计算某年某月的天数 
//----------------------------------------------- 
//int days = DateTime.DaysInMonth(2007, 8);
//days = 31; 
//给日期增加一天、减少一天 
//----------------------------------------------- 
//DateTime dt = DateTime.Now;
//dt.AddDays(1); //增加一天 
//dt.AddDays(-1);//减少一天 
//其它年份方法类似...
//Oracle SQL里转换日期函数
//----------------------------------------------- 
//to_date("2007-6-6",'YYYY-MM-DD"); 
//to_date("2007/6/6",'yyyy/mm/dd"); 
//如下一组数据, 如何查找表里包含9月份的记录: 
//CGGC_STRATDATE CGGC_ENDDATE
//========================================= 
//2007-8-4 2007-9-5 
//2007-9-5 2007-9-20 
//2007-9-22 2007-10-5 
//SELECT * FROM TABLE 
//(TO_DATE('2007/9/1','yyyy/mm/dd') BETWEEN CGGC_STRATDATE 
//AND CGGC_ENDDATE OR CGGC_STRATDATE >=TO_DATE('2007/9/1','yyyy/mm/dd') 
//AND CGGC_ENDDATE<=TO_DATE('2007/9/30','yyyy/mm/dd') " 
//OR TO_DATE('2007/9/30','yyyy/mm/dd') BETWEEN CGGC_STRATDATE 
//AND CGGC_ENDDATE) ORDER BY CGGC_STRATDATE ASC