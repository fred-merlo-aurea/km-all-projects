using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Content
{
    public class PersonalizedContent
    {

        public static ECN_Framework_Entities.Content.PersonalizedContent GetByBlastID_EmailAddress(Int64 BlastID, string EmailAddress)
        {
            ECN_Framework_Entities.Content.PersonalizedContent pc = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                pc = ECN_Framework_DataLayer.Content.PersonalizedContent.GetByBlastID_EmailAddress(BlastID, EmailAddress);
                scope.Complete();
            }
            return pc;
        }


        public static ECN_Framework_Entities.Content.PersonalizedContent GetByPersonalizedContentID(Int64 PersonalizedContentID)
        {
            ECN_Framework_Entities.Content.PersonalizedContent pc = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                pc = ECN_Framework_DataLayer.Content.PersonalizedContent.GetByPersonalizedContentID(PersonalizedContentID);
                scope.Complete();
            }
            return pc;
        }

        public static Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> GetNotProcessed()
        {
            Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> dpc = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dpc = ECN_Framework_DataLayer.Content.PersonalizedContent.GetNotProcessed();
                scope.Complete();
            }
            return dpc;
        }

        public static Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> GetDictionaryByPersonalizedContentIDs(string xmlPersonalizedContentIDs)
        {
            Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> dpc = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dpc = ECN_Framework_DataLayer.Content.PersonalizedContent.GetDictionaryByPersonalizedContentIDs(xmlPersonalizedContentIDs);
                scope.Complete();
            }
            return dpc;
        }

        public static void UpdateProcessed(ECN_Framework_Entities.Content.PersonalizedContent pc)
        {
            Dictionary<long, ECN_Framework_Entities.Content.PersonalizedContent> dpc = null;
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Content.PersonalizedContent.UpdateProcessed(pc);
                scope.Complete();

            }
        }



        public static int Save(ECN_Framework_Entities.Content.PersonalizedContent pc, KMPlatform.Entity.User currentUser)
        {
            int retID = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retID = ECN_Framework_DataLayer.Content.PersonalizedContent.Save(pc);
                scope.Complete();
            }
            return retID;

        }

        public static void MarkAsFailed(long pcID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Content.PersonalizedContent.MarkAsFailed(pcID);
                scope.Complete();
            }
        }
    }
}
