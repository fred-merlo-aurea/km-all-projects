using System;
using System.Collections.Generic;
using KMDbManagers;
using KMEntities;
using KMModels;

namespace KMManagers
{
    public class ThirdPartyQueryValueManager : ManagerBase
    {
        private ThirdPartyQueryValueDbManager QM
        {
            get
            {
                return DB.ThirdPartyQueryValueDbManager;
            }
        }

        public IEnumerable<TModel> GetAllByFormID<TModel>(int id) where TModel : ModelBase, new()
        {
            return QM.GetAllByFormID(id).ConvertToModels<TModel>();
        }

        public static void CopyAllByFR(FormResult r, int newId, Dictionary<int, int> lstControls)
        {
            ThirdPartyQueryValueManager manager = new ThirdPartyQueryValueManager();
            foreach (var v in r.ThirdPartyQueryValues)
            {
                ThirdPartyQueryValue newV = new ThirdPartyQueryValue();
                newV.FormResult_Seq_ID = newId;
                newV.Control_ID = lstControls[v.Control_ID];
                newV.Name = v.Name;
                manager.QM.Add(newV);
            }
            manager.QM.SaveChanges();
        }
    }
}