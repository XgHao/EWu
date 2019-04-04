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
    /*
    public class Identity
    {
        public void IdentityORC(string base64)
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
        }

        public static bool CheckValidationResult(object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors errors)
        {
            return true;
        }

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
                }
            }

        }

        public static string BuildQuery(IDictionary<string,string> parameters,string encode)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
        }

        public static string GetResponseAsString(HttpWebResponse rsp,Encoding encoding)
        {

        }
    }
    */
}