using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ECN_EngineFramework
{
    public static class EmailDirectHelper
    {
        public const string DefaultFromName          = "ECN_EngineFramework";
        public const string DefaultReplyToAddress    = "dev-group@knowledgemarketing.com";

        public static int SendEmail(
            string toEmailAddress, 
            string emailSubject, 
            string emailBody,
            string sourceApplication,
            string fromName            = DefaultFromName, 
            string replyToEmailAddress = DefaultReplyToAddress,
            [CallerMemberName]string sourceProcess = "" // auto-filled as calling method if not provided
            )
        {
            ECN_Framework_Entities.Communicator.EmailDirect message = new ECN_Framework_Entities.Communicator.EmailDirect
            {
                CustomerID = 1, // KM
                Status = "Pending",
                SendTime = DateTime.Now,
                EmailAddress = toEmailAddress,
                EmailSubject = emailSubject,
                Content = emailBody,
                FromName = fromName,
                ReplyEmailAddress = replyToEmailAddress,
                Source = sourceApplication,
                Process = sourceProcess.Contains(sourceApplication) ? sourceProcess : sourceApplication + "." + sourceProcess
            };
            return ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(message);
        }

        public static async Task<int> SendEmailAsync(
            string toEmailAddress, 
            string emailSubject, 
            string emailBody,
            string sourceApplication,
            string fromName            = DefaultFromName, 
            string replyToEmailAddress = DefaultReplyToAddress,
            [CallerMemberName] string sourceProcess = "" //auto-filled as calling method if not provided
            )
        {
            return await Task<int>.Run(() => SendEmail(toEmailAddress, emailSubject, emailBody, sourceApplication, fromName, replyToEmailAddress, sourceProcess));
        }
    }
}
