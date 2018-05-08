using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class RequestQueryUrlDbManager : DbManagerBase
    {
        public IEnumerable<RequestQueryValue> GetAllByRuleID(int ruleId)
        {
            return KM.RequestQueryValues.Where(x => x.Rule.Rule_Seq_ID == ruleId);
        }
       

        public void Add(RequestQueryValue newV)
        {
            KM.RequestQueryValues.Add(newV);
        }

        public void RewriteAll(int frId, IEnumerable<RequestQueryValue> values)
        {
            RewriteAll(frId, values, true);
        }

        public void RewriteAll(int rId, IEnumerable<RequestQueryValue> values, bool onlyAddNew)
        {
            if (!onlyAddNew)
            {
                List<RequestQueryValue> toRemove = KM.RequestQueryValues.Where(x => x.Rule_Seq_ID == rId).ToList();

                foreach (var v in toRemove)
                {
                    KM.RequestQueryValues.Remove(v);
                }
            }
            foreach (var v in values)
            {
                v.Rule_Seq_ID = rId;
                KM.RequestQueryValues.Add(v);
             
            }
         }

        public void Remove(ICollection<RequestQueryValue> queries)
        {
            foreach (var q in queries)
            {
                Remove(q);
            }
        }
        public void RemovebyRuleID(int Rule_Seq_ID)
        {
            IEnumerable<RequestQueryValue> op = GetAllByRuleID(Rule_Seq_ID);
            foreach (var o in op)
            {
                KM.RequestQueryValues.Remove(o);
            }
        }
        private void Remove(RequestQueryValue q)
        {
            KM.RequestQueryValues.Remove(KM.RequestQueryValues.Single(x => x.RequestQueryValue_Seq_ID == q.RequestQueryValue_Seq_ID));
        }
    }
}