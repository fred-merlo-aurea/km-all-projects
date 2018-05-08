using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace KM.Common.Utilities.Email
{
    public class EmailService
    {
        private const string ErrorMailSubject = "ErrorSubject";
        private const string ErrorMailToAddress = "ErrorNotification";
        private const string ErrorEmailFromAddress = "EmailFrom";
        private const string ErrorMailIsDemoField = "IsDemo";
        private const string HtmlNewLine = "<br />";

        private readonly IEmailClient _emailClient;
        private readonly IConfigurationProvider _configurationProvider;

        public EmailService(IEmailClient emailClient, IConfigurationProvider configurationProvider)
        {
            _emailClient = emailClient;
            _configurationProvider = configurationProvider;
        }

        public void SendEmail(EmailMessage emailMessage, string mailServer = null)
        {
            Guard.NotNull(emailMessage, nameof(emailMessage));

            var message = GetMailMessageFrom(emailMessage);
            _emailClient.Send(message, mailServer);            
        }

        public void SendEmail(string errorMessage, IList<Attachment> attachments = null, string mailServer = null)
        {
            var message = GetPreconfiguredMailMessage(errorMessage, attachments);
            _emailClient.Send(message, mailServer);
        }

        public void SendEmail(Exception exception, IList<Attachment> attachments = null, string mailServer = null)
        {
            Guard.NotNull(exception, nameof(exception));

            var formattedException = GetFormattedException(exception);
            var formattedInnerException = string.Empty;
            if (exception.InnerException != null)
            { 
                formattedInnerException = GetFormattedException(exception);
            }

            var emailBody =
                $"<html><body><h1>An Error Has Occurred!</h1>{formattedException}{formattedInnerException}</body></html>";
            var message = GetPreconfiguredMailMessage(emailBody, attachments);
            message.IsBodyHtml = true;

            _emailClient.Send(message, mailServer);
        }

        public void SaveEmail(EmailMessage emailMessage, string fileName)
        {
            Guard.NotNull(emailMessage, nameof(emailMessage));
            Guard.NotNullOrWhitespace(fileName, nameof(fileName));

            var message = GetMailMessageFrom(emailMessage);
            _emailClient.Save(message, fileName);
        }

        private MailMessage GetMailMessageFrom(EmailMessage message)
        {
            Guard.NotNull(message, nameof(message));
            Guard.NotNullOrWhitespace(message.From, nameof(message.From));
            Guard.NotNull(message.To, nameof(message.To));
            Guard.For(() => !message.To.Any(), () => new InvalidOperationException(nameof(message.To)));

            var mailMessage = new MailMessage();
            foreach (var toAddress in message.To)
            {
                mailMessage.To.Add(toAddress);
            }

            if (message.CarbonCopy != null)
            {
                foreach (var ccAddress in message.CarbonCopy)
                {
                    mailMessage.CC.Add(ccAddress);
                }
            }

            if (message.Attachments != null)
            {
                foreach (var attachment in message.Attachments)
                {
                    mailMessage.Attachments.Add(attachment);
                }
            }

            if (message.AlternateViews != null)
            {
                foreach (var alternateView in message.AlternateViews)
                {
                    mailMessage.AlternateViews.Add(alternateView);
                }
            }

            mailMessage.From = new MailAddress(message.From);
            mailMessage.Subject = message.Subject;
            mailMessage.Body = message.Body;
            mailMessage.IsBodyHtml = message.IsHtml;
            mailMessage.Priority = message.Priority;

            return mailMessage;
        }

        private MailMessage GetPreconfiguredMailMessage(string emailBody, IList<Attachment> attachments)
        {
            var configuredToAddress = _configurationProvider.GetValue<string>(ErrorMailToAddress);
            var configuredFromAddress = _configurationProvider.GetValue<string>(ErrorEmailFromAddress);
            var configuredSubject = _configurationProvider.GetValue<string>(ErrorMailSubject);
            var subject = $"IsDemo {_configurationProvider.GetValue<string>(ErrorMailIsDemoField)}";
            if (!string.IsNullOrWhiteSpace(configuredSubject))
            {
                subject = $"{subject} : {configuredSubject}";
            }

            var emailMessage = new EmailMessage();
            emailMessage.To.Add(configuredToAddress);
            emailMessage.Subject = subject;
            emailMessage.From = configuredFromAddress;
            emailMessage.Body = emailBody ?? string.Empty;
            emailMessage.IsHtml = false;

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    emailMessage.Attachments.Add(attachment);
                }
            }

            var message = GetMailMessageFrom(emailMessage);
            return message;
        }

        private string GetFormattedException(Exception exception)
        {
            Guard.NotNull(exception, nameof(exception));
            
            var errorTypeName = exception.GetType().ToString();
            var errorMessage = exception.Message;
            var errorSource = exception.Source;

            var errorStackTrace = string.Empty;
            if (!String.IsNullOrWhiteSpace(exception.StackTrace))
            {
                errorStackTrace = exception.StackTrace.Replace(Environment.NewLine, HtmlNewLine);
            }

            return
                @"<table cellpadding=""5"" cellspacing=""0"" border=""1"">" +
                $@"<tr><td style=""text-align: right;font-weight: bold"">URL:</td><td>:{errorSource}</td></tr>" +
                $@"<tr><td style=""text-align: right;font-weight: bold"">User:</td><td>General</td></tr>" +
                $@"<tr><td style=""text-align: right;font-weight: bold"">Exception Type:</td><td>{errorTypeName}</td></tr>" +
                $@"<tr><td style=""text-align: right;font-weight: bold"">Message:</td><td>{errorMessage}</td></tr>" +
                $@"<tr><td style=""text-align: right;font-weight: bold"">Stack Trace:</td><td>{errorStackTrace}</td></tr></table>";
        }
    }
}
