using APeiLee.Tool.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace APeiLee.Tool.Models
{
    public class ApiDataModel
    {
        /// <summary>
        /// 接口信息类
        /// </summary>
        /// <param name="httpRequestType">Http类型</param>
        /// <param name="urlAddress">接口地址</param>
        /// <param name="apiEncoding">字符编码 默认Encoding.UTF8</param>
        public ApiDataModel(HttpRequestTypeEnum httpRequestType, string urlAddress, Encoding? apiEncoding = null)
        {
            HttpRequestType = httpRequestType;
            UrlAddress = urlAddress;

            ApiEncoding = apiEncoding ?? Encoding.UTF8;
        }

        /// <summary>
        /// 请求类型Get\Post
        /// </summary>
        public HttpRequestTypeEnum HttpRequestType { get; set; }

        /// <summary>
        /// 请求接口地址
        /// </summary>
        public string UrlAddress { get; set; }

        /// <summary>
        /// 字符编码
        /// </summary>
        public Encoding ApiEncoding { get; set; }
    }
}
