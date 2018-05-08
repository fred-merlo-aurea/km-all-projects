using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class FileAudit
    {
        public List<Object.FileAudit> SelectDistinctProcessCodePerSourceFile(KMPlatform.Object.ClientConnections client)
        {
            List<Object.FileAudit> retList = null;
            retList = DataAccess.FileAudit.SelectDistinctProcessCodePerSourceFile(client);
            return retList;
        }
    }
}
