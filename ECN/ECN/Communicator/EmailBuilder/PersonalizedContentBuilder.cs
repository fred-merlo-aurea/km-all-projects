using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ecn.common.classes;
using ecn.communicator.classes;
using ECN_Framework_Entities.Content;
using KM.Common;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.Communicator.EmailBuilder
{
    public class PersonalizedContentBuilder
    {
        private const string EcnEncryptOpen = "ECN_Encrypt_Open";
        private const string EcnEncrypt = "ECN_Encrypt";
        private const string PersonalizedContentId = "PersonalizedContentID";
        private const string FailHtmlExceptionMessage = "Unable to build personalized html.";
        private const string FailTextExceptionMessage = "Unable to build personalized text.";
        private const char SplitChar = '_';
        private const string DoublePercentageSymbol = "%%";

        public static string ConvertHtml(MessageBuilder messageBuilder, PersonalizedContent personalizedContent)
        {
            Guard.NotNull(messageBuilder, nameof(messageBuilder));
            Guard.NotNull(personalizedContent, nameof(personalizedContent));

            var regex = new Regex(DoublePercentageSymbol); // Split on percents.
            var redirpage = GetRedirectionPage();
            var htmlBuilder = new StringBuilder();
            if (messageBuilder.EmailBlast.BlastEntity.EnableCacheBuster.HasValue &&
                messageBuilder.EmailBlast.BlastEntity.EnableCacheBuster.Value)
            {
                htmlBuilder.Append(TemplateFunctions.imgRewriter(personalizedContent.HTMLContent, messageBuilder.EmailBlast.BlastID));
            }
            else
            {
                htmlBuilder.Append(personalizedContent.HTMLContent);
            }

            htmlBuilder = new StringBuilder(Regex.Replace(htmlBuilder.ToString(),
                redirpage + "[^\"']*?lid=(\\d+)(?:&ulid=(\\d+))?&l=.*?([\"'])",
                m =>
                {
                    return string.IsNullOrEmpty(m.Groups[2].Value)
                        ? string.Format(@"{0}%%ECN_Encrypt_{1}%%{2}", redirpage, m.Groups[1].Value, m.Groups[3].Value)
                        : string.Format(@"{0}%%ECN_Encrypt_{1}_{2}%%{3}", redirpage, m.Groups[1].Value,
                            m.Groups[2].Value, m.Groups[3].Value);
                }, RegexOptions.Singleline | RegexOptions.Compiled));

            var contentCodeSnippets = regex.Split(htmlBuilder.ToString());
            htmlBuilder = new StringBuilder();
            for (var i = 0; i < contentCodeSnippets.Length; i++)
                try
                {
                    var lineData = contentCodeSnippets.GetValue(i).ToString();
                    if (i % 2 == 0)
                    {
                        htmlBuilder.Append(lineData);
                    }
                    else if (lineData.StartsWith("ECN_Encrypt"))
                    {
                        var items = lineData.Split(SplitChar);

                        var finalLink = string.Empty;
                        if (items.Count() > 3 && !string.IsNullOrWhiteSpace(items[3]))
                        {
                            finalLink =
                                $"b={messageBuilder.EmailBlast.BlastID}&e={messageBuilder.DataRow["EmailID"]}&lid={items[2]}&ulid={items[3]}";
                        }
                        else
                        {
                            finalLink =
                                $"b={messageBuilder.EmailBlast.BlastID}&e={messageBuilder.DataRow["EmailID"]}&lid={items[2]}";
                        }

                        finalLink = MessageBuilderHelper.GetFinalLinkEcnEncrypt(messageBuilder, finalLink);
                        htmlBuilder.Append(finalLink);
                    }
                    else
                    {
                        htmlBuilder.Append(messageBuilder.DataRow[lineData]);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(FailHtmlExceptionMessage, ex);
                }

            return htmlBuilder.ToString();
        }

        public static string ConvertText(MessageBuilder messageBuilder, PersonalizedContent personalizedContent)
        {
            Guard.NotNull(messageBuilder, nameof(messageBuilder));
            Guard.NotNull(personalizedContent, nameof(personalizedContent));

            var regex = new Regex(DoublePercentageSymbol); // Split on percents.
            var redirpage = GetRedirectionPage();
            var textBuilder = new StringBuilder();
            if (messageBuilder.EmailBlast.BlastEntity.EnableCacheBuster == true)
            {
                textBuilder.Append(TemplateFunctions.imgRewriter(personalizedContent.TEXTContent, messageBuilder.EmailBlast.BlastID));
            }
            else
            {
                textBuilder.Append(personalizedContent.TEXTContent);
            }

            var pattern = $"<{redirpage}[^>]*?lid=(\\d+)&l=.*?>";
            var replace = $@"<{redirpage}%%ECN_Encrypt_$1%%>";
            var newText = Regex.Replace(textBuilder.ToString(), pattern, replace, RegexOptions.Singleline);

            textBuilder = new StringBuilder(newText);

            var contentCodeSnippetsText = regex.Split(textBuilder.ToString());

            textBuilder = new StringBuilder();
            for (var i = 0; i < contentCodeSnippetsText.Length; i++)
            {
                try
                {
                    var lineData = contentCodeSnippetsText.GetValue(i).ToString();
                    if (i % 2 == 0)
                    {
                        textBuilder.Append(lineData);
                    }
                    else if (lineData.StartsWith(EcnEncrypt))
                    {
                        var items = lineData.Split(SplitChar);
                        var lid = items[2];

                        var finalLink = $"b={messageBuilder.GetBlastId()}&e={messageBuilder.GetEmailId()}&lid={lid}";

                        finalLink = MessageBuilderHelper.GetFinalLinkEcnEncrypt(messageBuilder, finalLink);

                        textBuilder.Append(finalLink);
                    }
                    else
                    {
                        textBuilder.Append(messageBuilder.DataRow[lineData]);
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(FailTextExceptionMessage, ex);
                }
            }

            return textBuilder.ToString();
        }

        public static bool HasPersonalizedContent(EmailBlast emailBlast, DataRow dataRow)
        {
            Guard.NotNull(emailBlast, nameof(emailBlast));
            Guard.NotNull(dataRow, nameof(dataRow));

            if (EqualsPersonalizationType(emailBlast.type))
            {
                var personalizedContentId = dataRow[PersonalizedContentId].ToString();

                if (!(string.IsNullOrWhiteSpace(personalizedContentId) || int.Parse(personalizedContentId) == 0))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool EqualsPersonalizationType(string blastType)
        {
            Guard.NotNull(blastType, nameof(blastType));
            return blastType.Equals(Enums.BlastType.Personalization.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static string GetRedirectionPage()
        {
            var redirpage = ConfigurationManager.AppSettings["Activity_DomainPath"] + "/engines/linkfrom.aspx";
            if (ConfigurationManager.AppSettings["OpenClick_UseOldSite"].ToLower().Equals("false"))
            {
                redirpage = ConfigurationManager.AppSettings["MVCActivity_DomainPath"] + "/Clicks/";
            }

            return redirpage;
        }
    }
}
