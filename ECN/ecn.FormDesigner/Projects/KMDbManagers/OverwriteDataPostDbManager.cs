using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;
using System.Data.Entity;

namespace KMDbManagers
{
    public class OverwriteDataPostDbManager : DbManagerBase
    {
        public IEnumerable<OverwritedataPostValue> GetAllByRuleID(int ruleId)
        {
            IEnumerable <OverwritedataPostValue> op = KM.OverwritedataPostValues.Where(x => x.Rule.Rule_Seq_ID == ruleId);
            return op;
        }
        
        public void Add(OverwritedataPostValue newV)
        {
            KM.OverwritedataPostValues.Add(newV);
        }

       
        public void RewriteAll(int rId, IEnumerable<OverwritedataPostValue> values, IEnumerable<OverwritedataPostValue> addvalues, bool onlyAddNew)
        {
            if (!onlyAddNew)
            {
                foreach (var c in values)
                {
                   OverwritedataPostValue o = KM.OverwritedataPostValues.First(t => t.OverwritedataValue_Seq_ID == c.OverwritedataValue_Seq_ID);
                   KM.OverwritedataPostValues.Remove(o);
                }
            }


            foreach (var c in addvalues)
            {
                c.Rule_Seq_ID = rId;
                KM.OverwritedataPostValues.Add(c);
            }
        }

        public void Remove(ICollection<OverwritedataPostValue> queries)
        {
            foreach (var q in queries)
            {
                Remove(q);
            }
        }
        public void RemovebyOwrSeqID(int O_Seq_Id)
        {
            KM.OverwritedataPostValues.Remove(KM.OverwritedataPostValues.Single(x => x.OverwritedataValue_Seq_ID == O_Seq_Id));
        }
        public void RemovebyRuleID(int Rule_Seq_ID)
        {
            IEnumerable<OverwritedataPostValue> op = GetAllByRuleID(Rule_Seq_ID);
            foreach (var o in op)
            {
                KM.OverwritedataPostValues.Remove(o);
            }              
        }
        public void Remove(OverwritedataPostValue q)
        {
            KM.OverwritedataPostValues.Remove(KM.OverwritedataPostValues.Single(x => x.OverwritedataValue_Seq_ID == q.OverwritedataValue_Seq_ID));
        }
    }
}