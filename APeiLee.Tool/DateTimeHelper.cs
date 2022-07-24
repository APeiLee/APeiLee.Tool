using System;
using System.Collections.Generic;
using System.Text;

namespace APeiLee.Tool
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 将Unix时间戳转换成时间格式，时间戳单位为：秒
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Int64 timestamp)
        {
            TimeSpan ts = TimeSpan.FromSeconds(timestamp);
            DateTime oldTime = new DateTime(1970, 1, 1);
            return oldTime.Add(ts);
        }

        /// <summary>
        /// 将Unix时间戳转换成时间格式，时间戳单位为：秒
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this Int32 timestamp)
        {
            TimeSpan ts = TimeSpan.FromSeconds(timestamp);
            DateTime oldTime = new DateTime(1970, 1, 1);
            return oldTime.Add(ts);
        }

        /// <summary>
        /// 将Unix时间戳转为本地时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeInLocal(this Int64 timestamp)
        {
            return timestamp.ToDateTime().ToLocalTime();
        }

        /// <summary>
        /// 将Unix时间戳转为北京时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeInBeijing(this Int64 timestamp)
        {
            return timestamp.ToDateTime().AddHours(8);
        }

        /// <summary>
        /// 时间字符串转时间
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string timeString)
        {
            if (DateTime.TryParse(timeString, out var resultDateTime))
            {
                return resultDateTime;
            }

            return null;
        }

        public static DateTime ToDateTime(this string valueStr, DateTime defaultDateTime)
        {
            DateTime resultValue;

            if (string.IsNullOrEmpty(valueStr))
            {
                resultValue = defaultDateTime;
            }

            else
            {
                if (!DateTime.TryParse(Convert.ToString(valueStr), out DateTime result))
                {
                    resultValue = defaultDateTime;
                }
                else
                {
                    resultValue = result;
                }
            }
            return resultValue;
        }

        /// <summary>
        /// ToString("yyyy-MM-dd HH:mm:ss")
        /// </summary>
        /// <param name="sourceDateTime"></param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime sourceDateTime)
        {
            return sourceDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 时间格式转为Unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static Int64 ToUnixTimestamp(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
}
