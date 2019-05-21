using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using Ewu.Domain.Db;
//腾讯云短信
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;
using System.Net.Mail;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.API
{
    public class Identity
    {
        #region 身份证IRC识别
        /// <summary>
        /// 身份证OCR识别
        /// </summary>
        /// <param name="base64"></param>
        /// <returns>返回结果字典集</returns>
        public Dictionary<string, string> IdentityORC(string base64)
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

            //保存结果的字典集
            Dictionary<string, string> result = new Dictionary<string, string>();

            //响应状态-请求不成功
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                result.Add("Status", "ERROR");
            }
            //请求成功
            else
            {
                result.Add("Status", "SUCCESS");
                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                JObject personInfo = JObject.Parse(reader.ReadToEnd());
                result.Add("address", personInfo["address"].ToString() ?? string.Empty);
                result.Add("birth", personInfo["birth"].ToString() ?? string.Empty);
                result.Add("name", personInfo["name"].ToString() ?? string.Empty);
                result.Add("nationality", personInfo["nationality"].ToString() ?? string.Empty);
                result.Add("num", personInfo["num"].ToString() ?? string.Empty);
                result.Add("sex", personInfo["sex"].ToString() ?? string.Empty);
            }
            return result;
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
        #endregion

        #region 手机验证

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="code">前端生成的验证码</param>
        /// <param name="phoNum">发送的手机号</param>
        /// <returns>发送结果</returns>
        public string MobileMsg(string code,string phoNum)
        {
            //短信应用SDK AppID
            int appid = 1400187647;

            //短信应用SDK AppKey
            string appkey = "225561ebc612eb400a62819edd1f192e";

            //需要发送短信的手机号码
            string[] phoneNumbers = { phoNum };

            //短信模板ID，需要在短信应用中申请
            int templateId = 281441;

            //签名
            string smsSign = "郑兴豪个人开发测试";

            //发送短信
            try
            {
                SmsSingleSender ssender = new SmsSingleSender(appid, appkey);
                var info = ssender.sendWithParam("86", phoneNumbers[0], templateId, new[] { code, "30" }, smsSign, "", "");
                string result = info.errMsg;
                //返回结果
                return result;
            }
            catch (JSONException e)
            {
                throw e;
            }
            catch (HTTPException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region 邮箱真实性

        /// <summary>
        /// 验证邮箱真实性
        /// </summary>
        /// <param name="mail">验证的电子邮箱地址</param>
        /// <returns></returns>
        public Dictionary<string, string> ValidEmail(string mail)
        {
            const string host = "https://emailcert.market.alicloudapi.com";
            const string path = "/email";
            const string method = "GET";
            const string appcode = "4b677fa79cc14896a78936a7dde89445";

            string querys = "email=" + mail;
            string bodys = "";
            string url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
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

            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            JObject emailInfo = JObject.Parse(reader.ReadToEnd());
            Dictionary<string, string> emailres = new Dictionary<string, string>();
            emailres.Add("Status", emailInfo["status"].ToString() ?? string.Empty);
            emailres.Add("Msg", emailInfo["msg"].ToString() ?? string.Empty);
            return emailres;
        }

        #endregion

        #region 发送邮件
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="email">目标地址</param>
        /// <param name="code">验证码</param>
        public void SendMail(string email, string code)
        {
            const string EwuEmail = "ewu@beishui.xyz";
            const string EwuPd = "Zxh10916114";

            //接收人邮箱
            MailAddress toMail = new MailAddress(email);
            MailAddress fromMail = new MailAddress(EwuEmail,"易物");

            //实例化一个发送邮件类
            MailMessage mailMessage = new MailMessage(fromMail, toMail);

            //标题
            mailMessage.Subject = "【易物】验证码";

            //内容
            mailMessage.Body = "您的验证码为:" + code + "，如不是本人操作，请忽略！";

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.exmail.qq.com";
            client.UseDefaultCredentials = false;
            client.EnableSsl = false;
            //client.Port = 465;
            client.Credentials = new NetworkCredential(EwuEmail, EwuPd);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(mailMessage);
        }
        #endregion

        #region IP地址归属地查询
        /// <summary>
        /// IP地址归属地查询
        /// </summary>
        /// <param name="ip"></param>
        public string GetIPAttribution(string ip)
        {
            if (ip != "::1")
            {
                const string host = "https://api01.aliyun.venuscn.com";
                const string path = "/ip";
                const string method = "GET";
                const string appcode = "4b677fa79cc14896a78936a7dde89445";

                string querys = "ip=" + ip;
                string bodys = "";
                string url = host + path;
                HttpWebRequest httpRequest = null;
                HttpWebResponse httpResponse = null;

                if (0 < querys.Length)
                {
                    url = url + "?" + querys;
                }

                if (host.Contains("https://"))
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

                var v1 = httpResponse.StatusCode;
                var v2 = httpResponse.Method;
                var v3 = httpResponse.Headers;
                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                JObject ipAttribution = JObject.Parse(reader.ReadToEnd());
                //查询失败
                if (ipAttribution["ret"].ToString() != "200")
                {
                    return ip + " 未知";
                }
                //获取IP归属地
                string city = ipAttribution["data"]["region"].ToString() + ipAttribution["data"]["city"].ToString();
                return ip + " " + city;
            }
            return ip + " 本地IP";
        }

        #endregion

        #region 全国快递查询
        public Delivery GetDeliveryInfo(string DeliveryNum)
        {
            const string host = "https://goexpress.market.alicloudapi.com";
            const string path = "/goexpress";
            const string method = "GET";
            const string appcode = "4b677fa79cc14896a78936a7dde89445";

            string querys = "no=" + DeliveryNum + "&type=";
            string bodys = "";
            string url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
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

            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            JObject DeliveryInfo = JObject.Parse(reader.ReadToEnd());

            //结果保存在字典集
            Dictionary<string, string> result = new Dictionary<string, string>();
            
            //新建一个快递信息集合
            Delivery delivery = new Delivery();
            
            //查询成功
            if (DeliveryInfo["code"].ToString() == "OK")
            {
                //添加信息
                delivery.code = DeliveryInfo["code"].ToString();
                delivery.msg = DeliveryInfo["msg"].ToString();
                delivery.name = DeliveryInfo["name"].ToString();
                delivery.No = DeliveryInfo["no"].ToString();
                #region State设置
                switch (DeliveryInfo["state"].ToString())
                {
                    case "-1":
                        delivery.StateInfo = "单号或代码错误";
                        break;
                    case "0":
                        delivery.StateInfo = "暂无轨迹";
                        break;
                    case "1":
                        delivery.StateInfo = "快递收件";
                        break;
                    case "2":
                        delivery.StateInfo = "在途中";
                        break;
                    case "3":
                        delivery.StateInfo = "签收";
                        break;
                    case "4":
                        delivery.StateInfo = "问题件";
                        break;
                    case "5":
                        delivery.StateInfo = "疑难件";
                        break;
                    case "6":
                        delivery.StateInfo = "退件签收";
                        break;
                    case "7":
                        delivery.StateInfo = "快递收件（揽件）";
                        break;
                    default:
                        delivery.StateInfo = "未知";
                        break;
                }
                #endregion
                #region 物流信息集合
                //新建一个快递信息List
                List<DeliveryInfo> deliveryInfos = new List<DeliveryInfo>();
                //获取所有list
                var DeliveryInfoList = DeliveryInfo["list"];
                //遍历所有信息
                foreach(var item in DeliveryInfoList)
                {
                    deliveryInfos.Add(new DeliveryInfo
                    {
                        time = item["time"].ToString(),
                        content = item["content"].ToString()
                    });
                }
                delivery.deliveryInfos = deliveryInfos.AsEnumerable();
                #endregion
            }
            else
            {
                delivery.msg = DeliveryInfo["msg"].ToString();  
            }
            return delivery;
        }
        #endregion


        #region 通知留言增加
        /// <summary>
        /// 添加留言通知
        /// </summary>
        /// <param name="RecipientID">接收人ID</param>
        /// <param name="SponsorID">发起人ID</param>
        /// <param name="NoticeObject">主题(类别)</param>
        /// <param name="NoticeContent">内容</param>
        /// <param name="TreasureUID">物品UID（可选）</param>
        /// <returns></returns>
        public void AddNotice(string RecipientID, string SponsorID, string NoticeObject, string NoticeContent, string TreasureUID = null, string RelpyNoticeUID = null)
        {
            using (var db = new NoticeDataContext())
            {
                db.Notice.InsertOnSubmit(new Domain.Db.Notice
                {
                    IsRead = false,
                    NoticeContent = NoticeContent,
                    NoticeObject = NoticeObject,
                    NoticeTime = DateTime.Now,
                    NoticeUID = Guid.NewGuid(),
                    RecipientID = RecipientID,
                    SponsorID = SponsorID,
                    TreasureUID = TreasureUID,
                    RelpyNoticeUID = RelpyNoticeUID
                });
                db.SubmitChanges();
            }
        }
        #endregion
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}