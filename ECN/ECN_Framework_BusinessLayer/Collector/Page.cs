using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Collector
{
    [Serializable]
    public class Page
    {
        public static List<ECN_Framework_Entities.Collector.Page> GetBySurveyID(int SurveyID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Collector.Page> itemList = new List<ECN_Framework_Entities.Collector.Page>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Collector.Page.GetBySurveyID(SurveyID);
                scope.Complete();
            }

            if (itemList.Count > 0)
            {
                foreach (ECN_Framework_Entities.Collector.Page item in itemList)
                {
                    item.QuestionList = ECN_Framework_BusinessLayer.Collector.Question.GetByPageID(item.PageID, user);
                }
            }
            return itemList;
        }

        public static int Save(ECN_Framework_Entities.Collector.Page Page, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Page.PageID = ECN_Framework_DataLayer.Collector.Page.Save(Page);
                scope.Complete();
            }

            return Page.PageID;
        }

        public static void Reorder(int PageID, string Position, int ToPageID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Collector.Page.Reorder(PageID, Position, ToPageID);
                scope.Complete();
            }
        }

        public static ECN_Framework_Entities.Collector.Page GetByPageID(int PageID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Collector.Page item;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Collector.Page.GetByPageID(PageID);
                scope.Complete();
            }
            if (item!=null)
            {
                item.QuestionList = ECN_Framework_BusinessLayer.Collector.Question.GetByPageID(item.PageID, user);
            }
            return item;
        }

        public static void Delete(int PageID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Collector.Page.Delete(PageID);
                scope.Complete();
            }
        }

        public static bool BranchFromExists(int PageID, KMPlatform.Entity.User user)
        {
            bool BranchFromExists = false;
            List<ECN_Framework_Entities.Collector.Question> qList = ECN_Framework_BusinessLayer.Collector.Question.GetByPageID(PageID, user);
            foreach (ECN_Framework_Entities.Collector.Question q in qList)
            {
                if (ECN_Framework_BusinessLayer.Collector.SurveyBranching.Exists(q.QuestionID, user))
                {
                    BranchFromExists = true;
                    break;
                }
            }
            return BranchFromExists;
        }

        public static bool BranchToExists(int PageID, KMPlatform.Entity.User user)
        {
            bool BranchToExists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                BranchToExists = ECN_Framework_DataLayer.Collector.Page.BranchToExists(PageID);
                scope.Complete();
            }
            return BranchToExists;
        }
    }
}
