using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Ewu.WebUI.API
{
    public class Identity
    {
        public Dictionary<string,object> IdentityORC(string base64)
        {
            string url = "http://dm-51.data.aliyun.com/rest/160601/ocr/ocr_idcard.json";
            string appcode = "4b677fa79cc14896a78936a7dde89445";

            //如果输入带有inputs，设置为True，否则设为False
            bool is_old_format = false;

            //配置格式
            string config = "{\\\"side\\\":\\\"face\\\"}";

            //访问方法
            string method = "POST";

            string querys = "";
            string bodys = string.Empty;
            if (is_old_format)
            {
                bodys = "{\"inputs\" :" +
                                    "[{\"image\" :" +
                                        "{\"dataType\" : 50," +
                                         "\"dataValue\" :\"" + base64 + "\"" +
                                         "}";
                if (config.Length > 0)
                {
                    bodys += ",\"configure\" :" +
                                    "{\"dataType\" : 50," +
                                     "\"dataValue\" : \"" + config + "\"}" +
                                     "}";
                }
                bodys += "]}"; 
            }
            else
            {
                bodys = "{\"image\":\"" + base64 + "\"";
                if (config.Length > 0)
                {
                    bodys += ",\"configure\" :\"" + config + "\"";
                }
                bodys += "}";
            }
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (url.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            //根据API的要求，定义相对应的Content-Type
            httpRequest.ContentType = "application/json; charset=UTF-8";
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            Dictionary<string, object> result = new Dictionary<string, object>();

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                result.Add("httpResponse", httpRequest);
                //ViewBag.httpResponse = httpResponse;
                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                result.Add("reader2", reader);
                //ViewBag.reader2 = reader.ReadToEnd();
                //Console.WriteLine(reader.ReadToEnd());
            }
            else
            {

                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                result.Add("reader2", reader.ReadToEnd());

                //ViewBag.reader2 = reader.ReadToEnd();
                //Console.WriteLine(reader.ReadToEnd());
            }

            return result;
        }

        public static bool CheckValidationResult(object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// Http(GET/POST)
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="paramters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>相应内容</returns>
        public static string sendPost(string url,IDictionary<string,string> paramters,string method)
        {
            if (method.ToLower() == "post")
            {
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                Stream reqStream = null;
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = method;
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(paramters, "utf8"));
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse)req.GetResponse();
                    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (reqStream != null)
                        reqStream.Close();
                    if (rsp != null)
                        rsp.Close();
                }
            }
            else
            {
                //创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(paramters, "utf8"));

                //GTE请求
                request.Method = "GET";
                request.ReadWriteTimeout = 5000;
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf8"));

                //返回内容
                string retString = myStreamReader.ReadToEnd();
                return retString;
            }

        }

        /// <summary>
        /// 组装普通文本请求参数
        /// </summary>
        /// <param name="parameters">Key-Value形势请求参数字典</param>
        /// <param name="encode"></param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string,string> parameters,string encode)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                //忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 把相应流转换为文本
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public static string GetResponseAsString(HttpWebResponse rsp,Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                //以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                //释放资源
                if (reader != null)
                {
                    reader.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
        }
    }
}