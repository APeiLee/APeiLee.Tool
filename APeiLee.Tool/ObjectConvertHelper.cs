using System;
using System.Collections.Generic;
using System.Text;

namespace APeiLee.Tool
{
    /// <summary>
    /// 类属性帮助类
    /// </summary>
    public static class ObjectConvertHelper
    {
        /// <summary>
        /// 将对象中的string属性中的null值置为string.Empty
        /// 当targetObject本身为null时，不做处理直接返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static T StringNullToEmpty<T>(this T targetObject)
        {
            if (targetObject == null)
            {
                return targetObject;
            }

            var properties = typeof(T).GetProperties();

            foreach (var p in properties)
            {
                if (p.PropertyType == typeof(string))
                {
                    string? pValue = typeof(T).GetProperty(p.Name)?.GetValue(targetObject, null)?.ToString();

                    if (string.IsNullOrEmpty(pValue))
                    {
                        typeof(T).GetProperty(p.Name)?.SetValue(targetObject, string.Empty, null);
                    }
                }
            }

            return targetObject;
        }

        /// <summary>
        /// 将对象属性名和值转为字典集合
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Dictionary<string, dynamic> ToParameterDictionary(this object model)
        {
            Dictionary<string, dynamic> resultDictionary = new Dictionary<string, dynamic>();

            if (model == null)
            {
                return resultDictionary;
            }

            Type modelType = model.GetType();

            foreach (var p in modelType.GetProperties())
            {
                resultDictionary.Add(p.Name, p.GetValue(model, null));
            }

            return resultDictionary;
        }

        /// <summary>
        /// 转换成int32型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Int32 ToInt32(this object? value, Int32 defaultValue = 0)
        {
            if (value == null) return defaultValue;
            Int32 m;
            return Int32.TryParse(value.ToString(), out m) ? m : defaultValue;
        }

        /// <summary>
        /// Convert.ToBoolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object? value)
        {
            if (value == null)
            {
                return false;
            }

            return Convert.ToBoolean(value);
        }
    }
}
