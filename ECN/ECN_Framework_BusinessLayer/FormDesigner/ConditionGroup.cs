using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using KMPlatform.Entity;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.FormDesigner
{
    [Serializable]
    public class ConditionGroup
    {
        public static ECN_Framework_Entities.FormDesigner.ConditionGroup GetByCondGroupID(int CondGroupID, bool fillChildren = false)
        {
            ECN_Framework_Entities.FormDesigner.ConditionGroup retItem = new ECN_Framework_Entities.FormDesigner.ConditionGroup();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retItem = ECN_Framework_DataLayer.FormDesigner.ConditionGroup.GetByConditionGroupID(CondGroupID);
                scope.Complete();
            }

            if(retItem != null && retItem.ConditionGroup_Seq_ID > 0 && fillChildren)
            {
                retItem.Conditions = ECN_Framework_BusinessLayer.FormDesigner.Condition.GetByCondGroupID(retItem.ConditionGroup_Seq_ID);
                retItem.ConditionGroup1 = ECN_Framework_BusinessLayer.FormDesigner.ConditionGroup.GetByMainGroupID(retItem.ConditionGroup_Seq_ID, fillChildren);
            }
            return retItem;
        }

        public static List<ECN_Framework_Entities.FormDesigner.ConditionGroup> GetByMainGroupID(int mainGroupID, bool fillChildren = false)
        {
            List<ECN_Framework_Entities.FormDesigner.ConditionGroup> retList = null;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.FormDesigner.ConditionGroup.GetByMainGroupID(mainGroupID);
                scope.Complete();
            }

            if(fillChildren)
            {
                foreach (ECN_Framework_Entities.FormDesigner.ConditionGroup cg in retList)
                {
                    cg.Conditions = ECN_Framework_BusinessLayer.FormDesigner.Condition.GetByCondGroupID(cg.ConditionGroup_Seq_ID);
                    cg.ConditionGroup1 = ECN_Framework_BusinessLayer.FormDesigner.ConditionGroup.GetByMainGroupID(cg.ConditionGroup_Seq_ID, fillChildren);
                }
            }
            return retList;
        }

        public static void Delete(int condGroupID)
        {
            ECN_Framework_Entities.FormDesigner.ConditionGroup cgToDelete = GetByCondGroupID(condGroupID, false);
            List<ECN_Framework_Entities.FormDesigner.ConditionGroup> childrenToDelete = GetByMainGroupID(condGroupID);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_BusinessLayer.FormDesigner.Condition.DeleteByGroupID(cgToDelete.ConditionGroup_Seq_ID);
                foreach(ECN_Framework_Entities.FormDesigner.ConditionGroup cg in childrenToDelete)
                {
                    Delete(cg.ConditionGroup_Seq_ID);
                }
                ECN_Framework_DataLayer.FormDesigner.ConditionGroup.Delete(condGroupID);
                scope.Complete();
            }

        }

        

        public static int Save(ECN_Framework_Entities.FormDesigner.ConditionGroup cg)
        {
            int retId = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                retId = ECN_Framework_DataLayer.FormDesigner.ConditionGroup.Save(cg);
                scope.Complete();
            }
            return retId;
        }

        public static void RewriteAllExceptMain(int mainGrID, List<ECN_Framework_Entities.FormDesigner.ConditionGroup> groups, bool onlyAddNew)
        {
            if (!onlyAddNew)
            {
                RemoveAllExceptMain(mainGrID);
            }
            foreach (var gr in groups)
            {
                gr.MainGroup_ID = mainGrID;
                //RewriteAllExceptMain(gr);
                Save(gr);
                
            }
        }

        private static void RemoveAllExceptMain(int mainGroupID)
        {
            ECN_Framework_Entities.FormDesigner.ConditionGroup cgMain = ECN_Framework_BusinessLayer.FormDesigner.ConditionGroup.GetByCondGroupID(mainGroupID, true);
            RemoveMain(cgMain, false);
        }

        public static IEnumerable<int> RemoveMain(ECN_Framework_Entities.FormDesigner.ConditionGroup main, bool removeMain)
        {
            List<int> IDs = new List<int>();
            AddID(main, IDs, removeMain);
            List<ECN_Framework_Entities.FormDesigner.ConditionGroup> toRemove = new List<ECN_Framework_Entities.FormDesigner.ConditionGroup>();
            foreach (int i in IDs)
            {
                toRemove.Add(ECN_Framework_BusinessLayer.FormDesigner.ConditionGroup.GetByCondGroupID(i));
            }
            
            foreach (var gr in toRemove)
            {
                ECN_Framework_BusinessLayer.FormDesigner.ConditionGroup.Delete(gr.ConditionGroup_Seq_ID);
                ECN_Framework_BusinessLayer.FormDesigner.Condition.DeleteByGroupID(gr.ConditionGroup_Seq_ID);
            }

            return IDs;
        }

        private static void AddID(ECN_Framework_Entities.FormDesigner.ConditionGroup conditionGroup, List<int> IDs, bool removeMain)
        {
            if (removeMain)
            {
                IDs.Add(conditionGroup.ConditionGroup_Seq_ID);
            }
            foreach (var gr in conditionGroup.ConditionGroup1)
            {
                AddID(gr, IDs);
            }
        }

        private static void AddID(ECN_Framework_Entities.FormDesigner.ConditionGroup conditionGroup, List<int> IDs)
        {
            AddID(conditionGroup, IDs, true);
        }

        

    }
}
