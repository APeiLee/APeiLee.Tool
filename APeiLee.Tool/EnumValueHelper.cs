using System;

namespace APeiLee.Tool
{
    public static class EnumValueHelper
    {
        /// <summary>
        /// 获取枚举类型描述DescriptionAttribute的值，如果无描述信息则返回该枚举名
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string EnumDescription(this Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

            if (objs.Length == 0)
            {
                return str;
            }

            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }
    }
}
