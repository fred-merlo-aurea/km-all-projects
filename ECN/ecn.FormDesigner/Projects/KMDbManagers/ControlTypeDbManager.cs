using System;
using System.Collections.Generic;
using System.Linq;
using KMEntities;

namespace KMDbManagers
{
    public class ControlTypeDbManager : DbManagerBase
    {
        public ControlType GetPaidQueryStringByName(string name)
        {
            
            ControlType CT = KM.ControlTypes.SingleOrDefault(x => x.Name == name);
            return CT;
          }
    }

      
}