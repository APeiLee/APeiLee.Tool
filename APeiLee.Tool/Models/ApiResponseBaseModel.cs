using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace APeiLee.Tool.Models
{
    /// <summary>
    /// 请求返回信息基类
    /// </summary>
    public class ApiResponseBaseModel
    {
        public ApiResponseBaseModel()
        {
            ResponseString = "";
            StatusCode = null;
        }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string ResponseString { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }
    }
}
