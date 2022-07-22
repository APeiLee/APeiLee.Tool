using System;
using System.Collections.Generic;
using System.Text;

namespace APeiLee.Tool.Enums
{
    public enum WebRequestErrorEnum
    {
        /// <summary>
        /// 请求网络错误
        /// </summary>
        WebError = 0,

        /// <summary>
        /// 返回空内容
        /// </summary>
        EmptyResponse = 1,

        /// <summary>
        /// 解析返回内容错误
        /// </summary>
        ParseFailed = 2,
    }
}
