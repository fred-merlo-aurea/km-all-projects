using System;
using System.Collections.Generic;
using KMDbManagers;
using KMEntities;

namespace KMManagers
{
    public class ConditionManager : ManagerBase
    {
        private ConditionDbManager CM
        {
            get
            {
                return DB.ConditionDbManager;
            }
        }

        public static void CopyAllByForm(Form form, Dictionary<int, int> lstControls, Dictionary<int, int> lstCGroups, Dictionary<int, int> lstItems)
        {
            ConditionDbManager CM = new ConditionDbManager();
            foreach (var c in CM.GetAllByForm(form))
            {
                Condition newC = new Condition();
                newC.Control_ID = lstControls[c.Control_ID];
                newC.ConditionGroup_Seq_ID = lstCGroups[c.ConditionGroup_Seq_ID];
                newC.Operation_ID = c.Operation_ID;
                //newC.Value = c.Control.IsSelectable() ? lstItems[int.Parse(c.Value)].ToString() : c.Value;
                if (c.Control.IsSelectable())
                {
                    int value;
                    if (lstItems.TryGetValue(int.Parse(c.Value), out value))
                    {
                        newC.Value = value.ToString();
                    }
                    else
                    {
                        newC.Value = c.Value;
                    }
                }
                else
                {
                    newC.Value = c.Value;
                }
                CM.Add(newC);
            }
            CM.SaveChanges();
        }
    }
}