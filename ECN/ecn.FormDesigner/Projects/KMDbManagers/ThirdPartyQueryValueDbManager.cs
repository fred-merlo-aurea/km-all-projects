using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class ThirdPartyQueryValueDbManager : DbManagerBase
    {
        public IEnumerable<ThirdPartyQueryValue> GetAllByFormID(int formId)
        {
            return KM.ThirdPartyQueryValues.Where(x => x.FormResult.Form_Seq_ID == formId);
        }

        public void Add(ThirdPartyQueryValue newV)
        {
            KM.ThirdPartyQueryValues.Add(newV);
        }

        public void RewriteAll(int frId, IEnumerable<ThirdPartyQueryValue> values)
        {
            RewriteAll(frId, values, false);
        }

        public void RewriteAll(int frId, IEnumerable<ThirdPartyQueryValue> values, bool onlyAddNew)
        {
            if (!onlyAddNew)
            {
                List<ThirdPartyQueryValue> toRemove = KM.ThirdPartyQueryValues.Where(x => x.FormResult_Seq_ID == frId).ToList();
                foreach (var v in toRemove)
                {
                    KM.ThirdPartyQueryValues.Remove(v);
                }
            }
            foreach (var v in values)
            {
                v.FormResult_Seq_ID = frId;
                KM.ThirdPartyQueryValues.Add(v);
            }
        }

        public void Remove(ICollection<ThirdPartyQueryValue> queries)
        {
            foreach (var q in queries)
            {
                Remove(q);
            }
        }

        private void Remove(ThirdPartyQueryValue q)
        {
            KM.ThirdPartyQueryValues.Remove(KM.ThirdPartyQueryValues.Single(x => x.ThirdPartyQueryValue_Seq_ID == q.ThirdPartyQueryValue_Seq_ID));
        }
    }
}