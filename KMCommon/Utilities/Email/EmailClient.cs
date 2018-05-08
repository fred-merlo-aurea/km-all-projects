using System;
using System.Configuration;
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;
using System.Reflection;

namespace KM.Common.Utilities.Email
{
    public class EmailClient : IEmailClient
    {
        private const string MailFileExtension = "*.eml";
        private const string DefaultMailServer = "108.160.208.101";
        private const string MailServerConfigurationKey = "MailServer";
        private const string MailServerConfigurationSection = "system.net/mailSettings";
        private const string MailWriterTypeName = "System.Net.Mail.MailWriter";
        private const string MailWriterSendMethodName = "Send";
        private const string MailWriterCloseMethodName = "Close";
        private const string ErrorMessageInvalidConfiguration = "The mail settings could not be retrieved from config file.";

        public void Send(MailMessage message, string mailServer = null)
        {
            Guard.NotNull(message, nameof(message));
            SendMail(message, mailServer);
        }

        public bool SendAndSave(MailMessage message, string destinationFilePath, string mailServer = null)
        {
            Guard.NotNull(message, nameof(message));
            Guard.NotNullOrWhitespace(destinationFilePath, nameof(destinationFilePath));

            var emailFileName = string.Empty;
            var emailDestinationFolderPath = Path.GetTempPath();
            using (var fileSystemWatcher = new FileSystemWatcher(emailDestinationFolderPath, MailFileExtension))
            {
                fileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
                fileSystemWatcher.Created += (sender, eventArgs) => emailFileName = eventArgs.FullPath;
                fileSystemWatcher.EnableRaisingEvents = true;

                SendMail(
                    message,
                    mailServer,
                    client =>
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        client.PickupDirectoryLocation = Path.GetTempPath();
                    });
                
                fileSystemWatcher.EnableRaisingEvents = false;                
            }

            if (string.IsNullOrEmpty(emailFileName))
            {
                return false;
            }

            File.Move(emailFileName, destinationFilePath);
            return true;
        }

        public void SendUsingConfiguration(MailMessage message, string filePath, string mailServer = null)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configuration.GetSectionGroup(MailServerConfigurationSection) as MailSettingsSectionGroup;
            if (settings == null)
            {
                throw new InvalidOperationException(ErrorMessageInvalidConfiguration);
            }

            if (settings.Smtp.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                SendAndSave(message, filePath, mailServer);                
            }
            else
            {
                Send(message, mailServer);
            }
        }

        public void Save(MailMessage message, string filePath, string mailServer = null)
        {
            Guard.NotNull(message, nameof(message));
            Guard.NotNullOrWhitespace(filePath, nameof(filePath));

            var mailWriterType = typeof(SmtpClient).Assembly.GetType(MailWriterTypeName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var mailWriterContructor = mailWriterType.GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null, 
                    new[]
                    {
                        typeof(Stream)
                    },
                    null);

                if (mailWriterContructor == null)
                {
                    return;
                }

                var mailWriter = mailWriterContructor.Invoke(new object[] { fileStream });
                var sendMethod = typeof(MailMessage)
                    .GetMethod(MailWriterSendMethodName, BindingFlags.Instance | BindingFlags.NonPublic);
                if (sendMethod == null)
                {
                    return;
                }

                var sendMethodParameters = sendMethod.GetParameters();
                sendMethod.Invoke(
                    message,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    sendMethodParameters.Length == 2
                        ? new[] { mailWriter, true }
                        : new[] { mailWriter, true, true },
                    null);

                var closeMethod = mailWriter
                    .GetType()
                    .GetMethod(MailWriterCloseMethodName, BindingFlags.Instance | BindingFlags.NonPublic);

                if (closeMethod == null)
                {
                    return;
                }

                closeMethod.Invoke(
                    mailWriter,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[] { },
                    null);
            }
        }

        private void SendMail(MailMessage message, string mailServer, Action<SmtpClient> configureAction = null)
        {
            Guard.NotNull(message, nameof(message));

            using (var smtpClient = GetSmtpClient(mailServer))
            {
                configureAction?.Invoke(smtpClient);
                smtpClient.Send(message);

                foreach (var attachment in message.Attachments)
                {
                    attachment.Dispose();
                }
            }
        }

        private SmtpClient GetSmtpClient(string mailServer = null)
        {
            var host = mailServer;
            if (string.IsNullOrWhiteSpace(host))
            {
                host = DefaultMailServer;
                var configuredMailServer = ConfigurationManager.AppSettings[MailServerConfigurationKey];
                if (!string.IsNullOrWhiteSpace(configuredMailServer))
                {
                    host = configuredMailServer;
                }
            }
            
            return new SmtpClient(host);
        }
    }
}