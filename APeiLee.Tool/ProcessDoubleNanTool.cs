namespace APeiLee.Tool
{
    public static class ProcessDoubleNanTool
    {
        //Process class property double nan
        /// <summary>
        /// 处理结构体中的Double.Nan值，转换为defaultValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue">NAN需要转为的默认值</param>
        /// <returns></returns>
        public static T ProcessDoubleNanStruct<T>(T value, double defaultValue) where T : struct
        {
            object tempValue = value;
            //get all properties
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                //get property value
                var propertyValue = property.GetValue(tempValue);
                //if property value is double
                if (propertyValue is double)
                {
                    //if property value is nan
                    if (double.IsNaN((double)propertyValue))
                    {
                        //set property value to default value
                        property.SetValue(tempValue, defaultValue, null);
                    }
                }
            }

            value = (T)tempValue;
            return value;
        }

        /// <summary>
        /// 处理类中的Double.Nan值，转换为defaultValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue">NAN需要转为的默认值</param>
        /// <returns></returns>
        public static T ProcessDoubleNanClass<T>(T value, double defaultValue) where T : class
        {
            //get all properties
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                //get property value
                var propertyValue = property.GetValue(value);
                //if property value is double
                if (propertyValue is double)
                {
                    //if property value is nan
                    if (double.IsNaN((double)propertyValue))
                    {
                        //set property value to default value
                        property.SetValue(value, defaultValue);
                    }
                }
            }

            return value;
        }
    }
}