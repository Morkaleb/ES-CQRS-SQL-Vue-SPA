using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ops.Infra
{
    public class Emailer
    {
        public static void Email(string sender, string receiver, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage(sender, receiver);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "whitespot-ca.mail.eo.outlook.com";
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                client.Send(mail);
            }
            catch (Exception e)
            {
                var blah = e;
            }
        }
    }
}

