using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;

namespace Ewu.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "957553851@qq.com";
        public string MailFromAddress = "Admin@YiWu.zxh";
        public bool UseSsl = false;
        public string Username = "Admin@YiWu.zxh";
        public string UserPassWd = "526114";
        public string ServerName = "139.199.82.204";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Favorite favorite, ShippingDetails shippingDetails)
        {
            using(var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.UserPassWd);

                StringBuilder body = new StringBuilder()
                                    .AppendLine("A new order has been submitted")
                                    .AppendLine("---")
                                    .AppendLine("Items:");
                foreach(var line in favorite.Lines)
                {
                    var subtotal = line.Treasure.BrowseNum * line.Treasure.Favorite;
                    body.AppendFormat("{0} * {1}(subtotal:{2}", line.Treasure.BrowseNum, line.Treasure.Favorite, subtotal);
                }
                body.AppendFormat("Total order value:{0}", favorite.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2 ?? "")
                    .AppendLine(shippingDetails.Line3 ?? "")
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.State ?? "")
                    .AppendLine(shippingDetails.Country)
                    .AppendLine(shippingDetails.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift warp:{0}", shippingDetails.GiftWrap ? "Yes" : "No");

                StringBuilder subject = new StringBuilder()
                                        .AppendLine("【易物】验证码");
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailSettings.MailFromAddress);
                mailMessage.To.Add(emailSettings.MailToAddress);
                //mailMessage.Subject = subject.ToString();
                //mailMessage.Body = body.ToString();
                mailMessage.Subject = "zhuti";
                mailMessage.Body = "test";
                smtpClient.Send(mailMessage);
            }
        }
    }
}
