using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class ConditionDbManager : DbManagerBase
    {
        public void RewriteAll(int groupID, IEnumerable<Condition> conditions, bool onlyAddNew)
        {
            if (!onlyAddNew)
            {
                List<Condition> toRemove = KM.Conditions.Where(x => x.ConditionGroup_Seq_ID == groupID).ToList();
                foreach (var c in toRemove)
                {
                    KM.Conditions.Remove(c);
                }
            }
            foreach (var c in conditions)
            {
                c.ConditionGroup_Seq_ID = groupID;
                KM.Conditions.Add(c);
            }
        }

        public IEnumerable<Condition> GetAllByForm(Form form)
        {
            List<int> IDs = form.Controls.Select(x => x.Control_ID).ToList();

            return KM.Conditions.Where(x => IDs.Contains(x.Control_ID));
        }

        public void Add(Condition newC)
        {
            KM.Conditions.Add(newC);
        }
    }
}