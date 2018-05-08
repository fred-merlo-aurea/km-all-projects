using System;
using System.Collections.Generic;
using KMDbManagers;
using KMEntities;
using KMModels;

namespace KMManagers
{
    public class RequestQueryUrlManager : ManagerBase
    {
        private RequestQueryUrlDbManager QM
        {
            get
            {
                return DB.RequestQueryUrlDbManager;
            }
        }

        public IEnumerable<RequestQueryValue> GetAllByRuleID(int id)
        {
            return QM.GetAllByRuleID(id);
        }


    }
}