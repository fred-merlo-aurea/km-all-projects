using System;
using System.Collections.Generic;
using KMDbManagers;
using KMEntities;
using KMModels;

namespace KMManagers
{
    public class OverwriteDataPostManager : ManagerBase
    {
        private OverwriteDataPostDbManager QM
        {
            get
            {
                return DB.OverWriteDataPostDbManager;
            }
        }

        public IEnumerable<OverwritedataPostValue> GetAllByRuleID(int id) 
        {
            IEnumerable<OverwritedataPostValue> op = QM.GetAllByRuleID(id);
            return op;
        }
        public static void CopyAllByFR(Rule r, int newId, Dictionary<int, int> lstControls)
        {
            OverwriteDataPostManager manager = new OverwriteDataPostManager();
            foreach (var v in r.OverwritedataPostValues)
            {
                OverwritedataPostValue newV = new OverwritedataPostValue();
                newV.Rule_Seq_ID = newId;
                newV.Control_ID = lstControls[v.Control_ID];
                newV.Value = v.Value;
                manager.QM.Add(newV);
            }
            manager.QM.SaveChanges();
        }
    }
}