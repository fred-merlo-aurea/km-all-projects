using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ECN_Framework_Common.Objects
{
    [Serializable]
    public class EmailFunctions
    {
        //LicenseCheck licenseCheck;


        //public EmailFunctions()
        //{
        //    //licenseCheck = new LicenseCheck();
        //}

        //public void SimpleSend(string ToEmail, string FromEmail, string Subject, string Body)
        //{
        //    if (ToEmail.ToString().Trim() != string.Empty)
        //    {
        //        System.Net.Mail.MailMessage simpleMail = new System.Net.Mail.MailMessage();
        //        simpleMail.From = new System.Net.Mail.MailAddress(FromEmail.ToString());
        //        simpleMail.To.Add(ToEmail.ToString());
        //        simpleMail.Subject = Subject.ToString();
        //        simpleMail.Body = Body.ToString();
        //        simpleMail.IsBodyHtml = true;
        //        simpleMail.Priority = System.Net.Mail.MailPriority.Normal;

        //        System.Net.Mail.SmtpClient smtpclient = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SmtpServer"].ToString());
        //        smtpclient.Send(simpleMail);
        //    }
        //}
    }
}
