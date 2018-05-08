using System;
using System.Data;
using System.IO;
using System.Text;

namespace ecn.communicator.classes
{
    public class PlainTextEmailMessageProvider : EmailMessageProvider
    {
        public PlainTextEmailMessageProvider(string toEmailAddress, string fromName, string fromEmailAddress, string replyTo, string bounceAddress)
            : base(toEmailAddress, fromName, fromEmailAddress, replyTo, bounceAddress)
        {
        }
        protected override void WriteContentType(ref StringBuilder sw, string boundryTag)
        {
            //sw.Append("Content-Type: multipart/alternative; boundary=\"" + boundryTag + "\"");
            //sw.Append("\r\n");
        }

        protected override void WriteContent(DataRow dr, ref StringBuilder sw, string boundryTag, string textBody, string htmlBody)
        {
            // Do nothing for plain text email message.
            //sw.Append("\r\n");
            //sw.Append("--" + boundryTag);
            //sw.Append("\r\n");
            sw.Append("Content-Type: text/plain; charset=\"ISO-8859-1\"");
            sw.Append("\r\n");
            sw.Append("Content-Transfer-Encoding: quoted-printable");
            sw.Append("\r\n");
            sw.Append("\r\n");
            sw.Append(QuotedPrintable.Encode(textBody) + "\r\n");
            //sw.Append(textBody + "\r\n");
            sw.Append("\r\n");

            sw.Append("\r\n");
            //sw.Append("--" + boundryTag);
        }
    }
}