﻿using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZKFM.Core.Infrastructure
{
    public static class HttpHelper
    {

        public static async Task<string> NetEaseRequest(string url, object obj, string method = "GET")
        {
            try
            {
                var result = string.Empty;
                var requestType = method.Trim().ToUpper();
                var param = "";

                if (requestType == "GET" && obj != null)
                {
                    //如果是get请求 拼接url
                    param = obj.ParseQueryString();
                    var sep = url.Contains('?') ? "&" : "?";
                    url += sep + param;
                }
                else if (requestType == "POST" && obj != null)
                {
                    var json = obj.ToJson();
                    var cryptoreq = NetEaseCryptoHelper.EncryptedRequest(json);
                    param = "params=" + cryptoreq.@params.UrlEncode() + "&encSecKey=" + cryptoreq.encSecKey.UrlEncode();
                }


                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = requestType;
                request.Headers[HttpRequestHeader.Referer] = "http://music.163.com";
                request.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.152 Safari/537";
                request.ContinueTimeout = 1000 * 10;
                //request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
                request.Headers[HttpRequestHeader.Cookie] = "appver=1.5.2;";


                if (requestType == "POST")
                {
                    //post请求  写入数据
                    byte[] bs = Encoding.UTF8.GetBytes(param);
                    request.ContentType = "application/x-www-form-urlencoded";
                    using (Stream reqStream = await request.GetRequestStreamAsync())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                    }
                }
                using (var response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    result = GetResponseBody(response);
                }
                return result;

            }
            catch (System.Exception)
            {
                return null;
            }
        }

        private static string GetResponseBody(HttpWebResponse response)
        {
            string responseBody = string.Empty;
            var contentEncoding = response.Headers[HttpResponseHeader.ContentEncoding];
            if (contentEncoding != null && contentEncoding.ToLower().Contains("gzip"))
            {
                using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            else if (contentEncoding != null && contentEncoding.ToLower().Contains("deflate"))
            {
                using (DeflateStream stream = new DeflateStream(
                    response.GetResponseStream(), CompressionMode.Decompress))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            else
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            return responseBody;
        }


    }
}
