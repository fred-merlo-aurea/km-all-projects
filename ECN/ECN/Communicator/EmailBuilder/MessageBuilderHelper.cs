using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using ecn.communicator.classes;
using ECN_Framework_Common.Objects.Communicator;
using KM.Common;

namespace ECN.Communicator.EmailBuilder
{
    public class MessageBuilderHelper
    {
        private const string EcnEncryptOpen = "ECN_Encrypt_Open";
        private const string EcnEncrypt = "ECN_Encrypt";
        private const string EmailAddress = "domain_admin@teckman.com";
        private const string SendTo = "SendTo";
        private const string SmtpServerConfigName = "SMTPServer";
        private const string SubjectPrefix = "Engine";
        private const string QuestionMark = "?";
        private const string Slot1 = "slot1";
        private const char SplitChar = '_';
        
        public static void ReplaceTextSnippets(
            MessageBuilder messageBuilder,
            StringBuilder builder,
            string personalizedContent,
            string lineData)
        {
            Guard.NotNull(messageBuilder, nameof(messageBuilder));
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(personalizedContent, nameof(personalizedContent));
            Guard.NotNull(lineData, nameof(lineData));

            if (lineData.StartsWith(EcnEncryptOpen))
            {
                var finalLink = $"b={messageBuilder.GetBlastId()}&e={messageBuilder.GetEmailId()}";
                finalLink = GetFinalLinkEcnEncrypt(messageBuilder, finalLink);
                builder.Append(finalLink);
            }
            else if (lineData.StartsWith(EcnEncrypt))
            {
                var items = lineData.Split('_');
                var lid = items[2];

                var finalLink = $"b={messageBuilder.GetBlastId()}&e={messageBuilder.GetEmailId()}&lid={lid}";
                finalLink = GetFinalLinkEcnEncrypt(messageBuilder, finalLink);
                builder.Append(finalLink);
            }
            else
            {
                AppendSnippet(messageBuilder, builder, personalizedContent, lineData);
            }
        }

        public static void ReplaceHtmlSnippets(
            MessageBuilder messageBuilder,
            StringBuilder body,
            string personalizedContent,
            string lineData)
        {
            Guard.NotNull(messageBuilder, nameof(messageBuilder));
            Guard.NotNull(body, nameof(body));
            Guard.NotNull(personalizedContent, nameof(personalizedContent));
            Guard.NotNull(lineData, nameof(lineData));

            if (lineData.Equals(EcnEncryptOpen))
            {
                var finalLinkOpen = $"b={messageBuilder.GetBlastId()}&e={messageBuilder.GetEmailId()}";
                finalLinkOpen = GetFinalLinkEcnEncrypt(messageBuilder, finalLinkOpen);

                body.Append(finalLinkOpen);
            }
            else if (lineData.StartsWith(EcnEncrypt))
            {
                var items = lineData.Split(SplitChar);

                var finalLink = string.Empty;
                if (items.Count() > 3 && !string.IsNullOrWhiteSpace(items[3]))
                {
                    finalLink =
                        $"b={messageBuilder.GetBlastId()}&e={messageBuilder.GetEmailId()}&lid={items[2]}&ulid={items[3]}";
                }
                else
                {
                    finalLink =
                        $"b={messageBuilder.GetBlastId()}&e={messageBuilder.GetEmailId()}&lid={items[2]}";
                }

                finalLink = GetFinalLinkEcnEncrypt(messageBuilder, finalLink);
                body.Append(finalLink);
            }
            else
            {
                AppendSnippet(messageBuilder, body, personalizedContent, lineData);
            }
        }

        public static void AppendSnippet(
            MessageBuilder messageBuilder,
            StringBuilder body,
            string personalizedContent,
            string lineData)
        {
            Guard.NotNull(messageBuilder, nameof(messageBuilder));
            Guard.NotNull(body, nameof(body));
            Guard.NotNull(personalizedContent, nameof(personalizedContent));
            Guard.NotNull(lineData, nameof(lineData));

            if (
                PersonalizedContentBuilder.EqualsPersonalizationType(messageBuilder.EmailBlast.type)
                && lineData.Equals(Slot1, StringComparison.OrdinalIgnoreCase))
            {
                body.Append(personalizedContent);
            }
            else
            {
                body.Append(messageBuilder.DataRow[lineData]);
            }
        }

        public static string GetFinalLinkEcnEncrypt(MessageBuilder messageBuilder, string finalLink)
        {
            Guard.NotNull(messageBuilder, nameof(messageBuilder));
            Guard.NotNull(finalLink, nameof(finalLink));

            var finalLinkEncrypted = string.Empty;

            if (!messageBuilder.UseOldLinks)
            {
                finalLinkEncrypted =
                    HttpUtility.UrlEncode(
                        Encryption.Base64Encrypt(finalLink, messageBuilder.EmailBlast.KMEncryption));
            }
            else
            {
                finalLinkEncrypted = $"{QuestionMark}{finalLink}";
            }

            return finalLinkEncrypted;
        }

        public static void AppendLines(DataRow dataRow, StringBuilder builder, Array lines)
        {
            Guard.NotNull(dataRow, nameof(dataRow));
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNull(lines, nameof(lines));

            for (var index = 0; index < lines.Length; index++)
            {
                var lineData = lines.GetValue(index).ToString();
                builder.Append(index % 2 == 0 ? lineData : HttpUtility.HtmlDecode(dataRow[lineData].ToString()));
            }
        }
    }
}
