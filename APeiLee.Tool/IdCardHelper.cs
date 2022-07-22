using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace APeiLee.Tool
{
    /// <summary>
    /// 中国居民身份证帮助类
    /// </summary>
    public static class IdCardHelper
    {
        /// <summary>
        /// 是否有效的身份证号码
        /// </summary>
        /// <param name="cardId">输入字符串</param>
        /// <returns>合法返回True</returns>
        public static bool IsCardId(string cardId)
        {
            if (string.IsNullOrEmpty(cardId))
            {
                return false;
            }

            if (!Regex.IsMatch(cardId, @"^([\d]{17,17}[\d|X]|[\d]{15,15})$"))
            {
                return false;
            }
            string[] provinceCodes = { "11", "12", "13", "14", "15",
                                    "21", "21", "22", "23",
                                    "31", "32", "33", "34", "35", "36", "37",
                                    "41", "42", "43", "44", "45", "46",
                                    "50", "51", "52", "53", "54",
                                    "61", "62", "63", "64", "65"};
            if (!provinceCodes.Contains(cardId.Substring(0, 2)))
            {
                return false;
            }

            if (cardId.Length == 15) cardId = cardId.Substring(0, 6) + "19" + cardId.Substring(6);
            DateTime? date = ConvertToDateTime(string.Format("{0}-{1}-{2}", cardId.Substring(6, 4), cardId.Substring(10, 2), cardId.Substring(12, 2)), null);
            if (date == null || date.Value > DateTime.Today)
            {
                return false;
            }
            if (cardId.Length == 17)
            {
                return true;
            }

            //加权因子常数
            int[] wi = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码
            char[] checksums = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

            int s = 0;
            for (int i = 0; i < 17; i++)
            {
                s += Convert.ToInt32(cardId[i].ToString()) * wi[i];
            }
            char checksum = checksums[s % 11];
            return cardId[17].Equals(checksum);
        }

        /// <summary>
        /// 将数字的字符串表示形式转换为它的等效 时间。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue">转换失败时替换的默认值（替换值必须在最小与最大参数范围内）</param>
        /// <returns></returns>
        private static DateTime? ConvertToDateTime(string value, DateTime? defaultValue = null)
        {
            DateTime? resultValue = null;

            if (string.IsNullOrEmpty(value))
            {
                resultValue = defaultValue;
            }

            else
            {
                if (!DateTime.TryParse(Convert.ToString(value), out DateTime result))
                {
                    resultValue = defaultValue;
                }
                else
                {
                    resultValue = result;
                }
            }
            return resultValue;
        }

        /// <summary>
        ///  根据省份证号码 区分性别
        ///  0:女，1:男，2:其他
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns>0:女，1:男，2:其他</returns>
        public static Int32 Sex(String idCard)
        {
            if (idCard.Length == 18)
            {
                return idCard.Substring(14, 3).ToInt32() % 2 == 1 ? 1 : 0;
            }
            if (idCard.Length == 15)
            {
                return idCard.Substring(12, 3).ToInt32() % 2 == 1 ? 1 : 0;
            }
            return 2;
        }

        /// <summary>
        /// 获取生日
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static string GetBirthDay(string idCard)
        {
            string birth = "";

            if (idCard.Length == 18)
            {
                birth = string.Format("{0}-{1}-{2}", idCard.Substring(6, 4), idCard.Substring(10, 2), idCard.Substring(12, 2));
            }
            else if (idCard.Length == 15)
            {
                birth = string.Format("{0}-{1}-{2}", "19" + idCard.Substring(6, 2), idCard.Substring(8, 2), idCard.Substring(10, 2));
            }

            var birthday = DateTime.MinValue;
            if (DateTime.TryParse(birth, out birthday))
            {
                return birth;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据出生日期计算年龄的方法
        /// </summary>
        /// <param name="birthDateStr">出生日期字符串</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static int CalculateAgeCorrect(string birthDateStr, DateTime now)
        {
            if (string.IsNullOrEmpty(birthDateStr))
            {
                return 0;
            }

            if (!DateTime.TryParse(birthDateStr, out DateTime birthDate))
            {
                return 0;
            }

            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
            return age;
        }

        /// <summary>
        /// 根据出生日期计算年龄的方法
        /// </summary>
        /// <param name="birthDate">出生日期时间</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static int CalculateAgeCorrect(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
            return age;
        }
    }
}
