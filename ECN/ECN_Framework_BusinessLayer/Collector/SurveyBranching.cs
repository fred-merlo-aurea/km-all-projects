using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Collector
{
    [Serializable]
    public class SurveyBranching
    {
        public static bool Exists(int QuestionID, KMPlatform.Entity.User user)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Collector.SurveyBranching.BranchingExits(QuestionID);
                scope.Complete();
            }
            return exists;
        }

        public static void DeletebyPageID(int PageID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ECN_Framework_DataLayer.Collector.SurveyBranching.DeletebyPageID(PageID);
                scope.Complete();
            }
        }

        public static void Save(ECN_Framework_Entities.Collector.SurveyBranching sb, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Collector.SurveyBranching.Save(sb, user.UserID);
                scope.Complete();
            }
        }

        public static ECN_Framework_Entities.Collector.SurveyBranching GetByOptionID(int OptionID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Collector.SurveyBranching item;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Collector.SurveyBranching.GetByOptionID(OptionID);
                scope.Complete();
            }
            return item;
        }
    }
}
