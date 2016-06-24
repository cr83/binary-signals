using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace EasyBot.Business
{
    public static class EmailSender
    {
        private static SmtpClient _client;
        private static SmtpClient Client {
            get
            {
                if (_client == null)
                {
                    _client = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
                    NetworkCredential credentials = new NetworkCredential("pikid.shop@gmail.com", "Czzykko1");
                    _client.Credentials = credentials;
                    _client.EnableSsl = true;
                }
                return _client;
            }
        }

        public static void SendMessage(MailMessage message)
        {
            Client.Send(message);
        }

        public static Task SendMessageAsync(string recipients, string subject, string text, string html)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("pikid.shop@gmail.com");
            msg.To.Add(recipients);
            msg.Subject = subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            return SendMessageAsync(msg);
        }

        public static Task SendMessageAsync(MailMessage message)
        {
            return Client.SendMailAsync(message);
        }
    }
}
