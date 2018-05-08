using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ecn.communicator.classes;
using ecn.communicator.classes.EmailWriter;
using ECN_Framework_Entities.Content;
using KM.Common;
using Encryption = KM.Common.Entity.Encryption;

namespace ECN.Communicator.EmailBuilder
{
    public class MessageBuilder
    {
        private const string KmCommonApplication = "KMCommon_Application";
        private const string ActivityDomainPath = "ActivityDomainPath";
        private const string EmailToFriendLink = "/engines/emailtofriend.aspx?e=";
        private const string EmailId = "EmailID";
        private const string GroupId = "GroupID";
        private const string PersonalizedContentId = "PersonalizedContentID";
        private const string SocialDomainPath = "SocialDomainPath";
        private const string SocialClick = "SocialClick";
        private const string FormatTypeCode = "FormatTypeCode";
        private const string EmailAddress = "EmailAddress";
        private const string BounceAddress = "BounceAddress";
        private const string Html = "html";
        private const string Text = "text";
        private const string EmailAddressDomain = "@enterprisecommunicationnetwork.com";
        private const string Communicator = "_=COMMUNICATOR=_";
        private const string HtmlRowBegin = "<tr>";
        private const string HtmlCellBegin = "<td>";
        private const string HtmlRowEnd = "</tr>";
        private const string HtmlCellEnd = "</td>";
        private const string Tab = "\t";
        private const string NewLine = "\n";
        private const char Splitter = '|';
        private const char UdfSplitter = ',';
        private const string DoubleNumberSign = "##";
        private const string DoubleDollarSign = "$$";
        private const string DoublePercentageSymbol = "%%";
        private const string TotalCountHtmlExceptionMessage =
            "Error Transnippet in HTML content. ContentTransnippet. CheckForTransnippet validation failed";
        private const string ModifyFailedHtmlExceptionMessage =
            "Error Transnippet in HTML content. ContentTransnippet.ModifyHTML Failed";
        private const string TotalCountTextExceptionMessage =
            "Error Transnippet in Text content. ContentTransnippet. CheckForTransnippet validation failed";
        private const string ModifyFailedTextExceptionMessage =
            "Error Transnippet in Text content.  ContentTransnippet.ModifyHTML Failed";

        public MessageBuilder(
            EmailBlast emailBlast,
            DataRow dataRow,
            Dictionary<long, PersonalizedContent> personalizedContent,
            DataRow[] emailProfileDataSet)
        {
            Guard.NotNull(emailBlast,nameof(emailBlast));
            Guard.NotNull(dataRow, nameof(dataRow));

            EmailBlast = emailBlast;
            DataRow = dataRow;
            EmailProfileDataSet = emailProfileDataSet;
            PersonalizedContent = personalizedContent;
            UseOldLinks = GetUseSiteFlag();
        }

        public readonly Dictionary<long, PersonalizedContent> PersonalizedContent;
        public readonly EmailBlast EmailBlast;
        public readonly DataRow DataRow;
        public readonly DataRow[] EmailProfileDataSet;
        public readonly MessageStringBuilders Builders = new MessageStringBuilders();
        public readonly bool UseOldLinks;
      
        public string GetMIMEMessage()
        {
            var message = string.Empty;
            try
            {
                BuildPersonalizedContent();
                BuildSocialMediaContent();
                BuildOldTransnippetsHtmlAndText();
                BuildNewVersionTransnippetsHtml();
                BuildNewVersionTransnippetsText();
                message = BuildMessage();
            }
            catch (Exception ex)
            {
                SendEmailNotification(ex);
                throw;
            }

            return message;
        }

        public int GetBlastId()
        {
            return EmailBlast.BlastID;
        }

        public string GetEmailId()
        {
            return DataRow[EmailId].ToString();
        }

        private string BuildMessage()
        {
            var message = string.Empty;

            var messageId =
                $"{EmailBlast.BlastID}.{DataRow[EmailId]}x{QuotedPrintable.RandomString(5, true)}{EmailAddressDomain}";
            var boundaryTag = $"{Communicator}{QuotedPrintable.RandomString(32, true)}".ToLower();

            var dynamicFromEmail = Builders.DynamicFromEmail.ToString().Trim();
            var messageFromEmailAddress = dynamicFromEmail.Length > 0
                ? dynamicFromEmail
                : EmailBlast.blast_msg.FromAddress;

            var dynamicFromName = Builders.DynamicFromName.ToString().Trim();
            var messageFromName = dynamicFromName.Length > 0
                ? Builders.DynamicFromName.ToString().Trim()
                : EmailBlast.blast_msg.FromName;

            var dynamicReplyTo = Builders.DynamicReplyTo.ToString().Trim();
            var messageReplyToEmailAddress = dynamicReplyTo.Length > 0
                ? dynamicReplyTo
                : EmailBlast.blast_msg.ReplyTo;


            if (EmailBlast.type.ToLower() == Text || DataRow[FormatTypeCode].ToString().ToLower() != Html)
            {
                var textMsgProvider = EmailMessageProvider.CreateInstance(Text, DataRow[EmailAddress].ToString(),
                    messageFromName, messageFromEmailAddress, messageReplyToEmailAddress,
                    DataRow[BounceAddress].ToString());

                message = textMsgProvider.WriteEmailMessage(
                    DataRow,
                    Builders.DynamicSubject.ToString(),
                    messageId,
                    Builders.TextBody.ToString(),
                    boundaryTag,
                    Builders.HtmlBody.ToString(),
                    EmailBlast.blastconfig);
            }
            else if (DataRow[FormatTypeCode].ToString().ToLower() == Html)
            {
                var htmlMsgProvider = EmailMessageProvider.CreateInstance(
                    Html,
                    DataRow[EmailAddress].ToString(),
                    messageFromName,
                    messageFromEmailAddress,
                    messageReplyToEmailAddress,
                    DataRow[BounceAddress].ToString());

                message = htmlMsgProvider.WriteEmailMessage(
                    DataRow,
                    Builders.DynamicSubject.ToString(),
                    messageId,
                    Builders.TextBody.ToString(),
                    boundaryTag,
                    Builders.HtmlBody.ToString(),
                    EmailBlast.blastconfig);
            }

            return message;
        }

        private void BuildPersonalizedContent()
        {
            var lBreakupSubject = EmailBlast.BreakupSubject;
            var personalizedContentHtml = string.Empty;
            var personalizedContentText = string.Empty;
            if (PersonalizedContentBuilder.HasPersonalizedContent(EmailBlast, DataRow))
            {
                var contendId = long.Parse(DataRow[PersonalizedContentId].ToString());
                var personalizedContent = PersonalizedContent[contendId];
                if (ContentTransnippet.CheckForTransnippet(Builders.HtmlBody.ToString()) <= 0)
                {
                    TransnippetHolder.TransnippetsCount = 0;
                }

                personalizedContentHtml = PersonalizedContentBuilder.ConvertHtml(this, personalizedContent);
                personalizedContentText = PersonalizedContentBuilder.ConvertText(this, personalizedContent);

                var r = new Regex(DoublePercentageSymbol);
                if (personalizedContent.EmailSubject != string.Empty)
                {
                    lBreakupSubject = r.Split(personalizedContent.EmailSubject);
                }
            }

            for (var index = 0; index < EmailBlast.BreakupHTMLMail.Length; index++)
            {
                var lineData = EmailBlast.BreakupHTMLMail.GetValue(index).ToString();
                if (index % 2 == 0)
                {
                    Builders.HtmlBody.Append(lineData);
                }
                else
                {
                    MessageBuilderHelper.ReplaceHtmlSnippets(
                        this,
                        Builders.HtmlBody,
                        personalizedContentHtml,
                        lineData);
                }
            }

            for (var index = 0; index < EmailBlast.BreakupTextMail.Length; index++)
            {
                var lineData = EmailBlast.BreakupTextMail.GetValue(index).ToString();
                if (index % 2 == 0)
                {
                    Builders.HtmlBody.Append(lineData);
                }
                else
                {
                    MessageBuilderHelper.ReplaceTextSnippets(
                        this,
                        Builders.TextBody,
                        personalizedContentText,
                        lineData);
                }
            }

            MessageBuilderHelper.AppendLines(DataRow, Builders.DynamicSubject, lBreakupSubject);
            MessageBuilderHelper.AppendLines(DataRow, Builders.DynamicFromEmail, EmailBlast.BreakupFromEmail);
            MessageBuilderHelper.AppendLines(DataRow, Builders.DynamicFromName, EmailBlast.BreakupFromName);
            MessageBuilderHelper.AppendLines(DataRow, Builders.DynamicReplyTo, EmailBlast.breakupReplyToEmail);
        }

        private void BuildSocialMediaContent()
        {
            if (EmailBlast.SocialShareUsed == null || EmailBlast.SocialShareUsed.Count <= 0)
            {
                return;
            }

            var encryption = Encryption.GetCurrentByApplicationID(
                Convert.ToInt32(ConfigurationManager.AppSettings[KmCommonApplication]));

            foreach (var socialShare in EmailBlast.SocialShareUsed)
            {
                if (socialShare.Key.Equals(5))
                {
                    UpdateLinks(Builders.HtmlBody, socialShare.Value);
                    UpdateLinks(Builders.TextBody, socialShare.Value);
                }
                else if (socialShare.Key != 4)
                {
                    if (encryption != null && encryption.ID > 0)
                    {
                        var queryString =
                            $"b={EmailBlast.BlastID}&g={DataRow[GroupId]}&e={DataRow[EmailId]}&m={socialShare.Key}";

                        var encryptedQuery =
                            HttpUtility.UrlEncode(KM.Common.Encryption.Encrypt(queryString, encryption));
                        var link =
                            $"{AppSettings(SocialDomainPath)}{AppSettings(SocialClick)}{encryptedQuery}";
                        Builders.HtmlBody.Replace(socialShare.Value, link);
                        Builders.TextBody.Replace(socialShare.Value, link);
                    }
                }

            }
        }

        private string AppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private StringBuilder UpdateLinks(StringBuilder builder, string socialShareValue)
        {
            return builder.Replace(socialShareValue,
                $"{AppSettings(ActivityDomainPath)}{EmailToFriendLink}{DataRow[EmailId]}&b={EmailBlast.BlastID}");
        }

        private void BuildOldTransnippetsHtmlAndText()
        {
            if (TransnippetHolder.TransnippetsCount > 0)
            {
                char[] splitter = {Splitter};
                char[] udfSplitter = {UdfSplitter};
                for (var index = 0; index < TransnippetHolder.Transnippet.Count; index++)
                {
                    var outputHtmlBuilder = new StringBuilder();
                    var outputTextBuilder = new StringBuilder();

                    var transnippetSplits = TransnippetHolder.Transnippet[index]
                        .Replace(DoubleNumberSign, string.Empty)
                        .Replace(DoubleDollarSign, string.Empty)
                        .Split(splitter);

                    if (transnippetSplits.Length > 1)
                    {
                        var transnippetUDFs = transnippetSplits[2].Split(udfSplitter);

                        if (EmailProfileDataSet.Length > 0 && transnippetUDFs.Length > 0)
                        {
                            foreach (var emailProfile in EmailProfileDataSet)
                            {
                                outputHtmlBuilder.Append(HtmlRowBegin);
                                outputTextBuilder.Append(NewLine);
                                foreach (var transnippet in transnippetUDFs)
                                {
                                    try
                                    {
                                        outputHtmlBuilder.Append(
                                            $"{HtmlCellBegin}{emailProfile[transnippet]}{HtmlCellEnd}");
                                        outputTextBuilder.Append($"{emailProfile[transnippet]}{Tab}");
                                    }
                                    catch
                                    {
                                        outputHtmlBuilder.Append($"{HtmlCellBegin}&nbsp;{HtmlCellEnd}");
                                        outputTextBuilder.Append(Tab);
                                    }
                                }

                                outputHtmlBuilder.Append(HtmlRowEnd);
                            }

                            Builders.HtmlBody.Replace(
                                TransnippetHolder.Transnippet[index],
                                TransnippetHolder.TransnippetTablesHTML[index].Replace(TransnippetHolder.Transnippet[index],
                                    outputHtmlBuilder.ToString()));

                            Builders.TextBody.Replace(
                                TransnippetHolder.Transnippet[index],
                                TransnippetHolder.TransnippetTablesTxt[index].Replace(TransnippetHolder.Transnippet[index],
                                    outputTextBuilder.ToString()));
                        }
                    }
                }
            }
        }

        private void BuildNewVersionTransnippetsHtml()
        {
            if (EmailBlast.HasTransnippets)
            {
                var emailRowsDT = new DataTable();

                var transTotalCount = ContentTransnippet.CheckForTransnippet(Builders.HtmlBody.ToString());
                if (transTotalCount == -1)
                {
                    Builders.HtmlBody.Remove(0, Builders.HtmlBody.Length);
                    throw new ArgumentException(TotalCountHtmlExceptionMessage);
                }

                if (transTotalCount > 0)
                {
                    var bfirst = true;
                    foreach (var row in EmailProfileDataSet)
                    {
                        if (bfirst)
                        {
                            bfirst = false;

                            foreach (DataColumn dc in row.Table.Columns)
                            {
                                emailRowsDT.Columns.Add(dc.ColumnName);
                            }
                        }

                        emailRowsDT.ImportRow(row);
                    }

                    try
                    {
                        var htmlModified = ContentTransnippet.ModifyHTML(Builders.HtmlBody.ToString(), emailRowsDT);
                        Builders.HtmlBody.Remove(0, Builders.HtmlBody.Length);
                        Builders.HtmlBody.Append(htmlModified);
                    }
                    catch (Exception ex)
                    {
                        Builders.HtmlBody.Remove(0, Builders.HtmlBody.Length);
                        throw new InvalidOperationException(ModifyFailedHtmlExceptionMessage, ex);
                    }
                }
            }
        }

        private void BuildNewVersionTransnippetsText()
        {
            if (EmailBlast.HasTransnippets)
            {
                var emailRowsDt = new DataTable();


                var transTotalCount = ContentTransnippet.CheckForTransnippet(Builders.TextBody.ToString());
                if (transTotalCount == -1)
                {
                    Builders.TextBody.Remove(0, Builders.TextBody.Length);
                    throw new ArgumentException(TotalCountTextExceptionMessage);
                }

                if (transTotalCount > 0)
                {
                    try
                    {
                        var textModified = ContentTransnippet.ModifyTEXT(Builders.TextBody.ToString(), emailRowsDt);
                        Builders.TextBody.Remove(0, Builders.TextBody.Length);
                        Builders.TextBody.Append(textModified);
                    }
                    catch (Exception ex)
                    {
                        Builders.TextBody.Remove(0, Builders.TextBody.Length);
                        throw new InvalidOperationException(ModifyFailedTextExceptionMessage,ex);
                    }
                }
            }
        }

        private void SendEmailNotification(Exception ex)
        {
            var original = ex;
            try
            {
                SendEmailNotification(
                    $"Error in GetMIMEMessage for BlastID: {EmailBlast.BlastID} EmailId: {DataRow[EmailId]}",
                    ex.ToString());
            }
            catch (Exception)
            {
                SendEmailNotification("Error in GetMIMEMessage", original.ToString());
            }

            Console.WriteLine(original.ToString());
        }

        private void SendEmailNotification(string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("domain_admin@teckman.com");
            message.To.Add(ConfigurationManager.AppSettings["SendTo"]);
            message.Subject = "Engine: " + System.AppDomain.CurrentDomain.FriendlyName.ToString() + " - " + subject;
            message.Body = body;


            SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"]);
            smtp.Send(message);
        }

        private bool GetUseSiteFlag()
        {
            var useOldLinks = Convert.ToBoolean(ConfigurationManager.AppSettings["OpenClick_UseOldSite"]);
            return useOldLinks;
        }
    }
}