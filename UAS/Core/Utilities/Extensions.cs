using System;
using System.Linq.Expressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core_AMS.Utilities
{
    public static class Extensions
    {
        #region Mail
        //public static void Save(this MailMessage Message, string FileName)
        //{
        //    Assembly assembly = typeof(SmtpClient).Assembly;
        //    Type _mailWriterType =
        //      assembly.GetType("System.Net.Mail.MailWriter");

        //    using (FileStream _fileStream =
        //           new FileStream(FileName, FileMode.Create))
        //    {
        //        // Get reflection info for MailWriter contructor
        //        ConstructorInfo _mailWriterContructor =
        //            _mailWriterType.GetConstructor(
        //                BindingFlags.Instance | BindingFlags.NonPublic,
        //                null,
        //                new Type[] { typeof(Stream) },
        //                null);

        //        // Construct MailWriter object with our FileStream
        //        object _mailWriter =
        //          _mailWriterContructor.Invoke(new object[] { _fileStream });

        //        // Get reflection info for Send() method on MailMessage
        //        MethodInfo _sendMethod =
        //            typeof(MailMessage).GetMethod(
        //                "Send",
        //                BindingFlags.Instance | BindingFlags.NonPublic);

        //        // Call method passing in MailWriter
        //        ParameterInfo[] api = _sendMethod.GetParameters();
        //        if (api.Length == 2)
        //        {
        //            _sendMethod.Invoke(
        //                Message,
        //                BindingFlags.Instance | BindingFlags.NonPublic,
        //                null,
        //                new object[] { _mailWriter, true },
        //                null);
        //        }
        //        else
        //        {
        //            _sendMethod.Invoke(
        //                Message,
        //                BindingFlags.Instance | BindingFlags.NonPublic,
        //                null,
        //                new object[] { _mailWriter, true, true },
        //                null);
        //        }
        //        //_sendMethod.Invoke(
        //        //    Message,
        //        //    BindingFlags.Instance | BindingFlags.NonPublic,
        //        //    null,
        //        //    new object[] { _mailWriter, true },
        //        //    null);

        //        // Finally get reflection info for Close() method on our MailWriter
        //        MethodInfo _closeMethod =
        //            _mailWriter.GetType().GetMethod(
        //                "Close",
        //                BindingFlags.Instance | BindingFlags.NonPublic);

        //        // Call close method
        //        _closeMethod.Invoke(
        //            _mailWriter,
        //            BindingFlags.Instance | BindingFlags.NonPublic,
        //            null,
        //            new object[] { },
        //            null);
        //    }
        //}
        //public static void Send(this SmtpClient emailer, MailMessage message, string fileName)
        //{
        //    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    var settings = config.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
        //    if (settings == null)
        //        throw new Exception("The mail settings could not be retrieved from config file.");

        //    if (settings.Smtp.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
        //    {
        //        var file = new FileInfo(fileName);
        //        var folder = file.Directory;
        //        if (folder == null)
        //            throw new DirectoryNotFoundException("folder");

        //        using (var watcher = new FileSystemWatcher(folder.FullName, "*.eml"))
        //        {
        //            watcher.NotifyFilter = NotifyFilters.FileName;
        //            watcher.Created += (sender, eventArgs) =>
        //            {
        //                File.Move(eventArgs.FullPath, fileName);
        //            };

        //            watcher.EnableRaisingEvents = true;
        //            emailer.Send(message);
        //            watcher.EnableRaisingEvents = false;
        //        }
        //    }
        //    else
        //    {
        //        emailer.Send(message);
        //    }
        //}
        #endregion
        #region HashSet

        #endregion
        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            MemberExpression body = (MemberExpression) expression.Body;
            return body.Member.Name;
        }
    }
}
