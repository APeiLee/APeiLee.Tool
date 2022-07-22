using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace APeiLee.Tool.Models
{
    /// <summary>
    /// 网络异常信息类
    /// </summary>
    [Serializable]
    public class NetWorkException : Exception
    {
        /// <summary>
        /// 网络状态码
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string? ApiUrl { get; set; }

        public override string? Message { get; }

        public NetWorkException()
        {

        }

        public NetWorkException(string message) : base(message)
        {

        }

        public NetWorkException(string message, Exception innerException) : base(message, innerException)
        {

        }

        [SecuritySafeCritical]
        public NetWorkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="responseModel">网络返回结果</param>
        public NetWorkException(GeneralResponseModel? responseModel) : base(responseModel?.Message)
        {
            if (responseModel == null)
            {
                return;
            }

            StatusCode = responseModel.StatusCode;
            ApiUrl = responseModel.Location;

            string statusCodeMsg = responseModel.StatusCode == null ? "" : $"HttpStatusCode={(int)StatusCode}，";

            Message = $"{statusCodeMsg}{responseModel.Message}";
        }
    }
}
