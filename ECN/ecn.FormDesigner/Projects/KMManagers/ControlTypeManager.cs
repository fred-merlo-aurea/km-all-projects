using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMManagers.APITypes;
using KMEnums;
using System.Data.SqlClient;
using KMDbManagers;
using KMEntities;
using KMModels;
using KMModels.PostModels;

namespace KMManagers
{
    public class ControlTypeManager : ManagerBase
    {
        private ControlTypeDbManager CTM
        {
            get
            {
                return DB.ControlTypeDbManager;
            }
        }

        public TModel GetPaidQueryStringByName<TModel>(string name) where TModel : ModelBase, new()
        {
            TModel res = null;
            KMEntities.ControlType ct = CTM.GetPaidQueryStringByName(name);
            if (ct != null)
            {
                res = ct.ConvertToModel<TModel>();
            }

            return res;
        }
    }
}
