using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class DataImportExport
    {
        public List<Entity.DataImportExport> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.DataImportExport> retList = null;
            retList = DataAccess.DataImportExport.Select(client);
            return retList;
        }
       
    }
}
