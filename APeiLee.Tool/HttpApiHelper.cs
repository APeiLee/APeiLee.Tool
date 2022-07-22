using APeiLee.Tool.Enums;
using APeiLee.Tool.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.CSharp;

namespace APeiLee.Tool
{
    public class HttpApiHelper
    {
        /// <summary>
        /// 接口根地址
        /// </summary>
        private readonly string _rootUrl;

        /// <summary>
        /// 取消下载标记
        /// </summary>
        private bool _cancelDownload;

        /// <summary>
        /// 接口权限信息
        /// </summary>
        private string _authorization;

        /// <summary>
        /// 设置接口权限信息
        /// </summary>
        /// <param name="authorization"></param>
        public void SetAuthorization(string authorization)
        {
            _authorization = "Bearer " + authorization;
        }

        #region 下载变化事件

        public delegate void DownloadChangedDelegate(int downloadPercent);
        public event DownloadChangedDelegate? DownloadChangedEvent;

        public delegate void DownloadErrorDelegate(string errorMsg);
        public event DownloadErrorDelegate? DownloadErrorEvent;

        public delegate void DownloadCompletedDelegate();
        public event DownloadCompletedDelegate? DownloadCompletedEvent;

        public delegate void DownloadCancelDelegate();
        public event DownloadCancelDelegate? DownloadCancelEvent;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rootUrl">API根地址</param>
        public HttpApiHelper(string rootUrl)
        {
            _rootUrl = AddHttpAndRemoveRightSlash(rootUrl);
            _authorization = "";
        }

        /// <summary>
        /// 通用处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="parameterModel"></param>
        /// <param name="contentType"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public T GenericRequest<T>(ApiDataModel apiData, object parameterModel, string contentType = "application/x-www-form-urlencoded", int timeout = 60000) where T : new()
        {
            GeneralResponseModel response = FormRequestAndAnalysisResponse(apiData, parameterModel?.ToParameterDictionary(), contentType, timeout);

            if (response != null && response.IsSuccess)
            {
                if (string.IsNullOrEmpty(response.Data?.ToString()))
                {
                    return default(T);
                }

                return response.Data.ToObject<T>();
            }

            throw new NetWorkException(response);
        }

        /// <summary>
        /// 通用处理，返回请求中的Data值(用于返回当Data不是Class时候的值)
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="parameterModel"></param>
        /// <param name="contentType"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public string GenericRequest_BackDataStr(ApiDataModel apiData, object parameterModel, string contentType = "application/x-www-form-urlencoded", int timeout = 60000)
        {
            GeneralResponseModel response = FormRequestAndAnalysisResponse(apiData, parameterModel?.ToParameterDictionary(), contentType, timeout);

            if (response != null && response.IsSuccess)
            {
                return response.Data?.ToString();
            }

            throw new NetWorkException(response);
        }

        /// <summary>
        /// 通用处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="parameterModel"></param>
        /// <param name="totalSize"></param>
        /// <param name="contentType"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public T GenericRequestList<T>(ApiDataModel apiData, object parameterModel,
            out int totalSize,
            string contentType = "application/x-www-form-urlencoded", int timeout = 60000) where T : new()
        {
            totalSize = 0;

            GeneralResponseModel response = FormRequestAndAnalysisResponse(apiData, parameterModel?.ToParameterDictionary(), contentType, timeout);

            if (response != null && response.IsSuccess)
            {
                if (string.IsNullOrEmpty(response.Data?.ToString()))
                {
                    return default(T);
                }

                ListDataResponseModel listDataModel = response.Data.ToObject<ListDataResponseModel>();

                totalSize = listDataModel.Total;

                if (string.IsNullOrEmpty(listDataModel.DataList?.ToString()))
                {
                    return default(T);
                }

                string dataListStr = listDataModel.DataList.ToString();

                return dataListStr.ToObject<T>();
            }

            throw new NetWorkException(response);
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="parameterModel"></param>
        /// <param name="contentType"></param>
        /// <param name="timeout"></param>
        public byte[] Download(ApiDataModel apiData, object parameterModel, string contentType = "application/x-www-form-urlencoded", int timeout = 60000)
        {
            try
            {
                _cancelDownload = false;

                return DownloadFileBytes(apiData, parameterModel?.ToParameterDictionary(), contentType, timeout);
            }
            catch (Exception e)
            {
                DownloadErrorEvent?.Invoke($"下载失败！错误消息={e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 取消下载
        /// </summary>
        public void CancelDownload()
        {
            //TODO 当一个实例类中有多个下载任务时会出问题

            _cancelDownload = true;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="fileKey"></param>
        /// <param name="fileContentType"></param>
        /// <param name="fileBytes"></param>
        /// <param name="parameterModel"></param>
        /// <param name="backFillName">接口返回的文件名称</param>
        /// <param name="contentType"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool UploadFileBytes(ApiDataModel apiData, string fileKey, string fileContentType, byte[] fileBytes,
            object parameterModel, out string backFillName, string contentType = "application/x-www-form-urlencoded",
            int timeout = 60000)
        {
            backFillName = "";

            var requestResponse = UploadFileBytesBase(apiData, fileKey, fileContentType, fileBytes, parameterModel?.ToParameterDictionary(), contentType, timeout);

            if (requestResponse != null && requestResponse.IsSuccess)
            {
                backFillName = requestResponse.Data?.ToString();

                return true;
            }

            return false;
        }

        /// <summary>
        /// 添加HTTP头，去掉结尾的右斜杠
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string AddHttpAndRemoveRightSlash(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("接口根地址为空");
            }

            while (url.EndsWith("/"))
            {
                url = url.Remove(url.Length - 1, 1);
            }

            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return url;
            }

            return "http://" + url;
        }

        /// <summary>
        /// 拼接表单字段数据
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        private static string GenerateParamsString(Dictionary<String, dynamic>? dic)
        {
            if (dic == null)
            {
                return "";
            }

            StringBuilder builder = new StringBuilder();
            IOrderedEnumerable<KeyValuePair<String, dynamic>> sortedDic = dic.OrderBy(o => o.Key);
            foreach (KeyValuePair<String, dynamic> pair in sortedDic)
            {
                builder.AppendFormat("{0}={1}&", pair.Key, ProcessSpecialChar(Convert.ToString(pair.Value)));
            }
            return builder.ToString().TrimEnd('&');
        }
        private static String ProcessSpecialChar(String paramStr)
        {
            return Uri.EscapeDataString(paramStr ?? "");
        }

        /// <summary>
        /// 表单请求
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="parameterDictionary"></param>
        /// <param name="contentType"></param>
        /// <param name="timeout">60*1000</param>
        /// <returns></returns>
        private ApiResponseBaseModel FormRequest(ApiDataModel apiData, Dictionary<String, dynamic>? parameterDictionary, string contentType = "application/x-www-form-urlencoded", int timeout = 60000)
        {
            ApiResponseBaseModel responseBaseModel = new ApiResponseBaseModel();

            HttpWebResponse httpWebResponse = GetHttpWebResponse(apiData, parameterDictionary, contentType, timeout);

            responseBaseModel.StatusCode = httpWebResponse.StatusCode;

            using (Stream responseStream = httpWebResponse.GetResponseStream())
            {
                if (responseStream != null)
                {
                    StreamReader streamReader = new StreamReader(responseStream, apiData.ApiEncoding);
                    responseBaseModel.ResponseString = streamReader.ReadToEnd();
                }
            }

            return responseBaseModel;
        }

        #region Download

        private byte[] DownloadFileBytes(ApiDataModel apiData, Dictionary<string, dynamic> parameterDictionary, string contentType = "application/x-www-form-urlencoded", int timeout = 60000)
        {
            byte[] resultBytes = null;

            long downloadCount = 0;

            HttpWebResponse httpWebResponse = GetHttpWebResponse(apiData, parameterDictionary, contentType, timeout);

            long downloadFileLength = httpWebResponse.ContentLength;

            //if (downloadFileLength > 104857600) //100*1024*1024 即100M
            //{
            //    throw new Exception($"下载文件过大！（当前文件大小={downloadFileLength}）该方法建议用于不超过100M的文件下载");
            //}

            using (Stream responseStream = httpWebResponse.GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (var fileMemoryStream = new MemoryStream((int)downloadFileLength))
                    {
                        int byteCount;
                        do
                        {
                            byte[] tempBuffer = new byte[10240];
                            byteCount = responseStream.Read(tempBuffer, 0, tempBuffer.Length);
                            fileMemoryStream.Write(tempBuffer, 0, byteCount);

                            //下载变化事件
                            downloadCount += byteCount;
                            DownloadChangedEvent?.BeginInvoke((int)(downloadCount * 1.0 / downloadFileLength * 100), null, null);
                        } while (byteCount > 0 && !_cancelDownload);

                        if (_cancelDownload)
                        {
                            //触发取消事件
                            DownloadCancelEvent?.Invoke();

                            return null;
                        }

                        resultBytes = fileMemoryStream.ToArray();

                        //下载完成事件
                        DownloadCompletedEvent?.Invoke();
                    }
                }
            }

            return resultBytes;
        }

        #endregion

        #region Upload

        private GeneralResponseModel UploadFileBytesBase(ApiDataModel apiData, string fileKey, string fileContentType, byte[] fileBytes, Dictionary<String, dynamic> parameterDictionary, string contentType = "application/x-www-form-urlencoded", int timeout = 60000)
        {
            ApiResponseBaseModel responseBaseModel = new ApiResponseBaseModel();

            HttpWebResponse httpWebResponse = UploadFileBytesRequestBase(apiData, fileKey, fileContentType, fileBytes, parameterDictionary, contentType, timeout);

            responseBaseModel.StatusCode = httpWebResponse.StatusCode;

            using (Stream responseStream = httpWebResponse.GetResponseStream())
            {
                if (responseStream != null)
                {
                    StreamReader responseStreamReader = new StreamReader(responseStream, apiData.ApiEncoding);
                    responseBaseModel.ResponseString = responseStreamReader.ReadToEnd();
                }
            }

            return AnalysisResponse(apiData, responseBaseModel);
        }

        #endregion

        #region 基本方法

        private HttpWebResponse GetHttpWebResponse(ApiDataModel apiData, Dictionary<String, dynamic>? parameterDictionary, string contentType = "application/x-www-form-urlencoded", int timeout = 60000)
        {
            var httpWebRequest = RequestBase(apiData, parameterDictionary, contentType, timeout);

            if (apiData.HttpRequestType == HttpRequestTypeEnum.Post)
            {
                string parameterString = GenerateParamsString(parameterDictionary);

                byte[] requestBytes = apiData.ApiEncoding.GetBytes(parameterString ?? "");

                if (requestBytes.Length > 0)
                {
                    httpWebRequest.ContentLength = requestBytes.Length;
                    using (Stream requestStream = httpWebRequest.GetRequestStream())
                    {
                        requestStream.Write(requestBytes, 0, requestBytes.Length);
                    }
                }
                else
                {
                    httpWebRequest.ContentLength = 0;
                }
            }

            return (HttpWebResponse)httpWebRequest.GetResponse();
        }

        private HttpWebResponse UploadFileBytesRequestBase(ApiDataModel apiData, string fileKey, string fileContentType, byte[] fileBytes, Dictionary<String, dynamic> parameterDictionary, string contentType = "application/x-www-form-urlencoded", int timeout = 60000)
        {
            var httpWebRequest = RequestBase(apiData, parameterDictionary, contentType, timeout);

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            //分割标记
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            //请求参数信息
            List<byte[]> parameterBytesList = new List<byte[]>();
            if (parameterDictionary != null && parameterDictionary.Count > 0)
            {
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in parameterDictionary.Keys)
                {
                    string formItem = String.Format(formdataTemplate, key, parameterDictionary[key]);
                    byte[] formItemBytes = System.Text.Encoding.UTF8.GetBytes(formItem);
                    parameterBytesList.Add(formItemBytes);
                }
            }

            //文件信息
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, fileKey, fileKey, fileContentType);
            byte[] headerbytes = Encoding.UTF8.GetBytes(header);

            //结束标记
            byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            //参数信息长度
            long parameterLength = 0;

            foreach (var itemBytese in parameterBytesList)
            {
                parameterLength += itemBytese.Length;
            }

            httpWebRequest.ContentLength = ((parameterBytesList.Count + 1) * boundaryBytes.Length) + parameterLength + headerbytes.Length + fileBytes.Length + trailer.Length;

            //开始写入
            Stream requestStream = httpWebRequest.GetRequestStream();

            foreach (var itemBytese in parameterBytesList)
            {
                requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                requestStream.Write(itemBytese, 0, itemBytese.Length);
            }

            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            requestStream.Write(headerbytes, 0, headerbytes.Length);
            requestStream.Write(fileBytes, 0, fileBytes.Length);
            requestStream.Write(trailer, 0, trailer.Length);
            requestStream.Close();

            return (HttpWebResponse)httpWebRequest.GetResponse();
        }

        private HttpWebRequest RequestBase(ApiDataModel apiData, Dictionary<String, dynamic> parameterDictionary, string contentType = "application/x-www-form-urlencoded", int timeout = 60000)
        {
            if (string.IsNullOrEmpty(_rootUrl))
            {
                throw new Exception("接口跟地址为空");
            }

            string parameterString = GenerateParamsString(parameterDictionary);

            string methodType;
            string requestUrl;

            switch (apiData.HttpRequestType)
            {
                case HttpRequestTypeEnum.Get:
                    methodType = WebRequestMethods.Http.Get;
                    contentType = "";
                    requestUrl = _rootUrl + apiData.UrlAddress + parameterString;
                    break;

                case HttpRequestTypeEnum.Post:
                    methodType = WebRequestMethods.Http.Post;
                    requestUrl = _rootUrl + apiData.UrlAddress;
                    break;

                default:
                    throw new Exception("不能处理该请求类型");
            }

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            httpWebRequest.Headers.Add("Authorization", _authorization);
            httpWebRequest.Method = methodType;
            httpWebRequest.ContentType = contentType;
            httpWebRequest.Timeout = timeout;

            return httpWebRequest;
        }

        /// <summary>
        /// 调用请求并包装返回数据
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="parameterDictionary"></param>
        /// <param name="contentType"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private GeneralResponseModel FormRequestAndAnalysisResponse(ApiDataModel apiData, Dictionary<String, dynamic>? parameterDictionary, string contentType = "application/x-www-form-urlencoded", int timeout = 60000)
        {
            ApiResponseBaseModel responseBaseModel = new ApiResponseBaseModel();

            try
            {
                responseBaseModel = FormRequest(apiData, parameterDictionary, contentType, timeout);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                string url = _rootUrl + apiData.UrlAddress;

                string errorInfoStr = e.Message;

                return new GeneralResponseModel()
                {
                    ErrorCode = (int)WebRequestErrorEnum.WebError,
                    Data = e.Message,
                    IsSuccess = false,
                    Location = url,
                    Message = errorInfoStr,
                    StatusCode = responseBaseModel.StatusCode ?? HttpStatusCode.InternalServerError,
                };
            }

            return AnalysisResponse(apiData, responseBaseModel);
        }

        private GeneralResponseModel AnalysisResponse(ApiDataModel apiData, ApiResponseBaseModel responseBaseModel)
        {
            if (responseBaseModel == null)
            {
                throw new Exception("接口返回基类信息为空");
            }

            //TODO 触发未授权
            //if (responseBaseModel.StatusCode == HttpStatusCode.Unauthorized)
            //{
            //}

            return ProcessResponseData(_rootUrl + apiData.UrlAddress, responseBaseModel);
        }

        /// <summary>
        /// 处理返回信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="responseModel"></param>
        /// <returns></returns>
        private GeneralResponseModel ProcessResponseData(string url, ApiResponseBaseModel responseModel)
        {
            if (responseModel == null)
            {
                responseModel = new ApiResponseBaseModel();
            }

            string responseStr = responseModel.ResponseString;

            if (string.IsNullOrWhiteSpace(responseStr) || responseStr[0] != '{')
            {
                return new GeneralResponseModel()
                {
                    ErrorCode = (int)WebRequestErrorEnum.EmptyResponse,
                    Data = responseStr,
                    IsSuccess = false,
                    Location = url,
                    Message = "返回内容非Json格式",
                    StatusCode = responseModel.StatusCode ?? HttpStatusCode.InternalServerError,
                };
            }

            try
            {
                return responseStr.ToObject<GeneralResponseModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return new GeneralResponseModel()
                {
                    ErrorCode = (int)WebRequestErrorEnum.ParseFailed,
                    Data = responseStr,
                    IsSuccess = false,
                    Location = url,
                    Message = "解析失败",
                    StatusCode = responseModel.StatusCode ?? HttpStatusCode.InternalServerError,
                };
            }
        }

        #endregion

    }
}
