using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class EmailActivityLog
    {
        public static ECN_Framework_Entities.Communicator.EmailActivityLog GetByBlastEAID(int eaid)
        {
            ECN_Framework_Entities.Communicator.EmailActivityLog eal = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                eal = ECN_Framework_DataLayer.Communicator.EmailActivityLog.GetByBlastEAID(eaid);
                scope.Complete();
            }

            return eal;
        }

        public static int Insert(ECN_Framework_Entities.Communicator.EmailActivityLog log, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                log.Processed = "n";
                log.EAID = ECN_Framework_DataLayer.Communicator.EmailActivityLog.Insert(log);
                scope.Complete();
            }
            EventOrganizer.Event(log, user);
            return log.EAID;
        }

        public static int Insert(int blastID, int emailID, string actionTypeCode, string actionValue, string actionNotes, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.EmailActivityLog log = new ECN_Framework_Entities.Communicator.EmailActivityLog();         
            log.BlastID = blastID;
            log.EmailID = emailID;
            log.ActionTypeCode = actionTypeCode;
            log.ActionValue = actionValue;
            log.ActionNotes = actionNotes;
            return Insert(log, user);
        }

        public static int InsertSeedClick(int blastID, int emailID, string actionTypeCode, string actionValue, string actionNotes, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.EmailActivityLog log = new ECN_Framework_Entities.Communicator.EmailActivityLog();
            log.BlastID = blastID;
            log.EmailID = emailID;
            log.ActionTypeCode = actionTypeCode;
            log.ActionValue = actionValue;
            log.ActionNotes = actionNotes;
            using (TransactionScope scope = new TransactionScope())
            {
                log.Processed = "n";
                log.EAID = ECN_Framework_DataLayer.Communicator.EmailActivityLog.InsertSeedClick(log);
                scope.Complete();
            }
            //EventOrganizer.Event(log, user);
            return log.EAID;
        }

        public static int InsertSeedOpen(int blastID, int emailID, string actionTypeCode, string actionValue, string actionNotes, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.EmailActivityLog log = new ECN_Framework_Entities.Communicator.EmailActivityLog();
            log.BlastID = blastID;
            log.EmailID = emailID;
            log.ActionTypeCode = actionTypeCode;
            log.ActionValue = actionValue;
            log.ActionNotes = actionNotes;
            using (TransactionScope scope = new TransactionScope())
            {
                log.Processed = "n";
                log.EAID = ECN_Framework_DataLayer.Communicator.EmailActivityLog.InsertSeedOpen(log);
                scope.Complete();
            }
            //EventOrganizer.Event(log, user);
            return log.EAID;
        }

        public static int InsertOpen(int EmailID, int BlastID, string spyInfo, KMPlatform.Entity.User user)
        {
            if (EmailID == -1)
            {
                return InsertSeedOpen(BlastID, EmailID, "open", spyInfo, "", user);
            }
            else
            {
            return Insert(BlastID, EmailID, "open", spyInfo, "", user);
        }
        }

        public static int InsertClick(int EmailID, int BlastID, string Link, string spyInfo, int UniqueLinkID, KMPlatform.Entity.User user)
        {
            //int opensCount = GetOpenCount(EmailID, BlastID);
            //if (opensCount > 0)
            //{
            //    return Insert(BlastID, EmailID, "click", Link, UniqueLinkID.ToString(), user);
            //}
            //else
            //{
            //    if (spyInfo.Length > 2048)
            //    {
            //        spyInfo = spyInfo.Substring(0, 2048);
            //    }
            //    Insert(BlastID, EmailID, "open", spyInfo, "", user);
            //    return Insert(BlastID, EmailID, "click", Link, UniqueLinkID.ToString(), user);
            //}
            if (EmailID == -1)
            {
                return InsertSeedClick(BlastID, EmailID, "click", Link, UniqueLinkID.ToString(), user);
            }
            else
            {
            return Insert(BlastID, EmailID, "click", Link, UniqueLinkID.ToString(), user);
        }
        }

        public static int InsertConversion(int EmailID, int BlastID, string Link, string spyInfo, KMPlatform.Entity.User user)
        {
            //int opensCount = GetOpenCount(EmailID, BlastID);
            //if (opensCount > 0)
            //{
            //    return Insert(BlastID, EmailID, "conversion", Link, "", user);
            //}
            //else
            //{
            //    if (spyInfo.Length > 2048)
            //    {
            //        spyInfo = spyInfo.Substring(0, 2048);
            //    }
            //    Insert(BlastID, EmailID, "open", spyInfo, "", user);
            //    return Insert(BlastID, EmailID, "conversion", Link, "", user);
            //}

            return Insert(BlastID, EmailID, "conversion", Link, "", user);
        }

        public static int InsertConversionRevenue(int EmailID, int BlastID, string Total, KMPlatform.Entity.User user)
        {
            return Insert(BlastID, EmailID, "conversionRevenue", Total, "", user);
        }

        public static int InsertRead(int EmailID, int BlastID, string spyInfo, KMPlatform.Entity.User user)
        {
            //int opensCount = GetOpenCount(EmailID, BlastID);
            //if (opensCount > 0)
            //{
            //    return Insert(BlastID, EmailID, "read", spyInfo, "", user);
            //}
            //else
            //{
            //    if (spyInfo.Length > 2048)
            //    {
            //        spyInfo = spyInfo.Substring(0, 2048);
            //    }
            //    Insert(BlastID, EmailID, "open", spyInfo, "", user);
            //    return Insert(BlastID, EmailID, "read", spyInfo, "", user);
            //}

            return Insert(BlastID, EmailID, "read", spyInfo, "", user);
        }

        public static int InsertForward(int EmailID, int BlastID, string EmailAddress, string spyInfo, KMPlatform.Entity.User user)
        {
            //int opensCount = GetOpenCount(EmailID, BlastID);
            //if (opensCount > 0)
            //{
            //    return Insert(BlastID, EmailID, "refer", EmailAddress, "", user);
            //}
            //else
            //{
            //    if (spyInfo.Length > 2048)
            //    {
            //        spyInfo = spyInfo.Substring(0, 2048);
            //    }
            //    Insert(BlastID, EmailID, "open", spyInfo, "", user);
            //    return Insert(BlastID, EmailID, "refer", EmailAddress, "", user);
            //}

            return Insert(BlastID, EmailID, "refer", EmailAddress, "", user);
        }

        public static int GetOpenCount(int EmailID, int BlastID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.EmailActivityLog.GetOpenCount(EmailID, BlastID);
                scope.Complete();
            }
            return count;
        }

        public static int GetConversionRevenueCount(int EmailID, int BlastID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.EmailActivityLog.GetConversionRevenueCount(EmailID, BlastID);
                scope.Complete();
            }
            return count;
        }

        public static int GetSendCount(int EmailID, int BlastID)
        {
            int count = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                count = ECN_Framework_DataLayer.Communicator.EmailActivityLog.GetSendCount(EmailID, BlastID);
                scope.Complete();
            }
            return count;
        }

        public static void InsertBounce(string xmlDoc, int defaultThreshold)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailActivityLog.InsertBounce(xmlDoc, defaultThreshold);
                scope.Complete();
            }
        }

        public static void InsertSpamFeedbackXML(string xmlDoc, string actionTypeCode)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailActivityLog.InsertSpamFeedbackXML(xmlDoc, actionTypeCode);
                scope.Complete();
            }
        }

        public static void InsertSpamFeedback(int BlastID, int EmailID, string Reason, string subscribeTypeCode, string actionTypeCode)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailActivityLog.InsertSpamFeedback(BlastID, EmailID, Reason, subscribeTypeCode, actionTypeCode);
                scope.Complete();
            }
        }

        public static void InsertOptOutFeedback(int BlastID, string Groups, int EmailID, string Reason)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailActivityLog.InsertOptOutFeedback(BlastID, Groups, EmailID, Reason);
                scope.Complete();
            }
        }
    }
}
