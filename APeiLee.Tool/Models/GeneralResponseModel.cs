using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.CSharp;

namespace APeiLee.Tool.Models
{
    /// <summary>
    /// 请求返回数据
    /// </summary>
    public class GeneralResponseModel
    {
        /// <summary>
        /// 返回网络状态码
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }

        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { get; set; }

        public string? Message { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string? Location { get; set; }

        public dynamic? Data { get; set; }
    }
}
