using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class ConditionGroupDbManager : DbManagerBase
    {
        public IEnumerable<int> Remove(IEnumerable<ConditionGroup> groups)
        {
            List<int> res = new List<int>();
            foreach (var g in groups)
            {
                res.AddRange(RemoveMain(g));
            }

            return res;
        }

        public void Remove(IEnumerable<int> IDs)
        {
            IEnumerable<ConditionGroup> lst = KM.ConditionGroups.Where(x => IDs.Contains(x.ConditionGroup_Seq_ID)).ToList();
            foreach (var gr in lst)
            {
                KM.ConditionGroups.Remove(gr);
            }
        }

        public IEnumerable<int> RemoveMain(int groupId)
        {
            return RemoveMain(KM.ConditionGroups.Single(x => x.ConditionGroup_Seq_ID == groupId));
        }

        public void RemoveAllExceptMain(int groupId)
        {
            RemoveMain(KM.ConditionGroups.Single(x => x.ConditionGroup_Seq_ID == groupId), false);
        }

        public IEnumerable<int> RemoveMain(ConditionGroup main)
        {
            return RemoveMain(main, true);
        }

        public IEnumerable<int> RemoveMain(ConditionGroup main, bool removeMain)
        {
            List<int> IDs = new List<int>();
            AddID(main, IDs, removeMain);
            List<ConditionGroup> toRemove = KM.ConditionGroups.Where(x => IDs.Contains(x.ConditionGroup_Seq_ID)).ToList();
            foreach (var gr in toRemove)
            {
                KM.ConditionGroups.Remove(gr);
            }

            return IDs;
        }

        private void AddID(ConditionGroup conditionGroup, List<int> IDs)
        {
            AddID(conditionGroup, IDs, true);
        }

        private void AddID(ConditionGroup conditionGroup, List<int> IDs, bool removeMain)
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

        public int AddNew(bool logic)
        {
            ConditionGroup gr = new ConditionGroup();
            gr.LogicGroup = logic;
            KM.ConditionGroups.Add(gr);
            SaveChanges();

            return gr.ConditionGroup_Seq_ID;
        }

        public int ResetLogicMethodByGroupID(int? id, bool logic)
        {
            int groupID = -1;
            if (id.HasValue)
            {
                groupID = id.Value;
                ResetLogicMethodByGroupID(id.Value, logic);
            }
            else
            {
                groupID = AddNew(logic);
            }

            return groupID;
        }

        public void ResetLogicMethodByGroupID(int id, bool logic)
        {
            KM.ConditionGroups.Single(x => x.ConditionGroup_Seq_ID == id).LogicGroup = logic;
        }

        public int CopyGroup(ConditionGroup conditionGroup, Dictionary<int, int> lst)
        {
            return CopyGroup(conditionGroup, null, lst);
        }

        private int CopyGroup(ConditionGroup conditionGroup, int? mainGroupId, Dictionary<int, int> lst)
        {
            ConditionGroup gr = new ConditionGroup();
            gr.MainGroup_ID = mainGroupId;
            gr.LogicGroup = conditionGroup.LogicGroup;
            KM.ConditionGroups.Add(gr);
            SaveChanges();
            lst.Add(conditionGroup.ConditionGroup_Seq_ID, gr.ConditionGroup_Seq_ID);

            foreach (var g in conditionGroup.ConditionGroup1)
            {
                CopyGroup(g, gr.ConditionGroup_Seq_ID, lst);
            }

            return gr.ConditionGroup_Seq_ID;
        }

        public IEnumerable<ConditionGroup> GetAllByRulesAndNotifications(IEnumerable<int> ruleIDs, IEnumerable<int> notificationIDs)
        {
            return KM.ConditionGroups.Where(x => ruleIDs.Contains(x.ConditionGroup_Seq_ID) || notificationIDs.Contains(x.ConditionGroup_Seq_ID));
        }

        public IEnumerable<int> GetConditionGroupIDsByControl(Control c)
        {
            List<int> mainGroups = new List<int>();
            foreach (var gr in c.Conditions.Select(x => x.ConditionGroup))
            {
                AddMainIDs(gr, mainGroups);
            }
            List<int> IDs = new List<int>();
            IDs.AddRange(mainGroups);
            IEnumerable<ConditionGroup> lst = KM.ConditionGroups.Where(x => IDs.Contains(x.ConditionGroup_Seq_ID));
            foreach (var gr in lst)
            {
                AddID(gr, IDs);
            }

            return IDs;
        }

        private void AddMainIDs(ConditionGroup gr, List<int> mainGroups)
        {
            while (gr.ConditionGroup2 != null)
            {
                gr = gr.ConditionGroup2;
            }
            if (!mainGroups.Contains(gr.ConditionGroup_Seq_ID))
            {
                mainGroups.Add(gr.ConditionGroup_Seq_ID);
            }
        }

        public void RewriteAllExceptMain(int mainGrID, IEnumerable<ConditionGroup> groups, bool onlyAddNew)
        {
            if (!onlyAddNew)
            {
                RemoveAllExceptMain(mainGrID);
            }
            foreach (var gr in groups)
            {
                gr.MainGroup_ID = mainGrID;
                //RewriteAllExceptMain(gr);
                KM.ConditionGroups.Add(gr);
                SaveChanges();
            }
        }

        //public void RewriteAllExceptMain(ConditionGroup newGr)
        //{
        //    KM.ConditionGroups.Add(newGr);
        //    SaveChanges();
        //    int newGrID = newGr.ConditionGroup_Seq_ID;

        //    foreach (var c in newGr.Conditions)
        //    {
        //        c.ConditionGroup_Seq_ID = newGrID;
        //        KM.Conditions.Add(c);
        //    }
        //    SaveChanges();
        //}
    }
}