using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Reflection;

namespace ADMS.Services.Emailer
{
    public static class MailMessageExt
    {
        public static void Save(this MailMessage Message, string FileName)
        {
            try
            {
                Assembly assembly = typeof(SmtpClient).Assembly;
                Type _mailWriterType =
                  assembly.GetType("System.Net.Mail.MailWriter");

                using (FileStream _fileStream =
                       new FileStream(FileName, FileMode.Create))
                {
                    // Get reflection info for MailWriter contructor
                    ConstructorInfo _mailWriterContructor =
                        _mailWriterType.GetConstructor(
                            BindingFlags.Instance | BindingFlags.NonPublic,
                            null,
                            new Type[] { typeof(Stream) },
                            null);

                    // Construct MailWriter object with our FileStream
                    object _mailWriter =
                      _mailWriterContructor.Invoke(new object[] { _fileStream });

                    // Get reflection info for Send() method on MailMessage
                    MethodInfo _sendMethod =
                        typeof(MailMessage).GetMethod(
                            "Send",
                            BindingFlags.Instance | BindingFlags.NonPublic);

                    // Call method passing in MailWriter
                    ParameterInfo[] api = _sendMethod.GetParameters();
                    if (api.Length == 2)
                    {
                        _sendMethod.Invoke(
                            Message,
                            BindingFlags.Instance | BindingFlags.NonPublic,
                            null,
                            new object[] { _mailWriter, true },
                            null);
                    }
                    else
                    {
                        _sendMethod.Invoke(
                            Message,
                            BindingFlags.Instance | BindingFlags.NonPublic,
                            null,
                            new object[] { _mailWriter, true, true },
                            null);
                    }
                    //_sendMethod.Invoke(
                    //    Message,
                    //    BindingFlags.Instance | BindingFlags.NonPublic,
                    //    null,
                    //    new object[] { _mailWriter, true },
                    //    null);

                    // Finally get reflection info for Close() method on our MailWriter
                    MethodInfo _closeMethod =
                        _mailWriter.GetType().GetMethod(
                            "Close",
                            BindingFlags.Instance | BindingFlags.NonPublic);

                    // Call close method
                    _closeMethod.Invoke(
                        _mailWriter,
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        new object[] { },
                        null);
                }
            }
            catch(Exception ex) 
            {
                //Logging logger = new Logging();
                //logger.LogIssue(ex);

                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine;
                if (FrameworkUAS.Object.AppData.myAppData != null && FrameworkUAS.Object.AppData.myAppData.CurrentApp != null)
                    app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                KMPlatform.BusinessLogic.ApplicationLog alWorker = new KMPlatform.BusinessLogic.ApplicationLog();
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alWorker.LogCriticalError(formatException, "ADMS.Services.Emailer.MailMessageExt.Save", app, string.Empty);
            }
        }
        public static void Send(this SmtpClient emailer, MailMessage message, string fileName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = config.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            if (settings == null)
                throw new Exception("The mail settings could not be retrieved from config file.");

            if (settings.Smtp.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                var file = new FileInfo(fileName);
                var folder = file.Directory;
                if (folder == null)
                    throw new DirectoryNotFoundException("folder");

                using (var watcher = new FileSystemWatcher(folder.FullName, "*.eml"))
                {
                    watcher.NotifyFilter = NotifyFilters.FileName;
                    watcher.Created += (sender, eventArgs) =>
                    {
                        File.Move(eventArgs.FullPath, fileName);
                    };

                    watcher.EnableRaisingEvents = true;
                    emailer.Send(message);
                    watcher.EnableRaisingEvents = false;
                }
            }
            else
            {
                emailer.Send(message);
            }
        }
    }
}
