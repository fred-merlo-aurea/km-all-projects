using System;
using System.Data;
using System.IO;
using System.Text;

namespace ecn.communicator.classes
{
    public class HtmlEmailMessageProvider : EmailMessageProvider
    {
        public HtmlEmailMessageProvider(string toEmailAddress, string fromName, string fromEmailAddress, string replyTo, string bounceAddress)
            : base(toEmailAddress, fromName, fromEmailAddress, replyTo, bounceAddress)
        {
        }

        protected override void WriteContentType(ref StringBuilder sw, string boundryTag)
        {
            sw.Append("Content-Type: multipart/alternative; boundary=\"" + boundryTag + "\"");
            sw.Append("\r\n");
            sw.Append("\r\n");
        }

        protected override void WriteContent(DataRow dr, ref StringBuilder sw, string boundryTag, string textBody, string htmlBody)
        {
            string QuotedPrintabletext = QuotedPrintable.Encode(textBody) + "\r\n";

            sw.Append(QuotedPrintabletext);

            // RFC 1521 says to order the links from "worst" to "best".. ie TEXT to HTML
            sw.Append("\r\n");
            sw.Append("--" + boundryTag);
            sw.Append("\r\n");

            sw.Append("Content-Type: text/plain; charset=\"ISO-8859-1\"");
            sw.Append("\r\n");
            sw.Append("Content-Transfer-Encoding: quoted-printable");
            sw.Append("\r\n");
            sw.Append("\r\n");
            sw.Append(QuotedPrintabletext);
            sw.Append("\r\n");

            sw.Append("\r\n");
            sw.Append("--" + boundryTag);
            sw.Append("\r\n");
            sw.Append("Content-Type: text/html; charset=\"ISO-8859-1\"");
            sw.Append("\r\n");
            sw.Append("Content-Transfer-Encoding: quoted-printable");
            sw.Append("\r\n");
            sw.Append("\r\n");

            sw.Append(QuotedPrintable.Encode(htmlBody) + "\r\n");

            sw.Append("--" + boundryTag + "--");
            sw.Append("\r\n");
            sw.Append("\r\n");
        }
    }
}
