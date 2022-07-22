using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace APeiLee.Tool
{
    /// <summary>
    /// Json转换帮助类
    /// </summary>
    public static class JsonConvertHelper
    {
        /// <summary>
        /// 将对象转为Json String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static String ToJson<T>(this T obj)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return JsonConvert.SerializeObject(obj, setting);
        }

        /// <summary>
        /// 将Json String转为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string obj)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            };
            return JsonConvert.DeserializeObject<T>(obj, setting);
        }

        /// <summary>
        /// 将UTF8编码字节数组转为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static T ToObject<T>(this byte[] buffer)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            };
            String jsonString = Encoding.UTF8.GetString(buffer);
            return JsonConvert.DeserializeObject<T>(jsonString, setting);
        }
    }
}
