using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class FileValidator_ImportError
    {
        public List<Entity.FileValidator_ImportError> Select(string processCode, int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FileValidator_ImportError> x = null;
            x = DataAccess.FileValidator_ImportError.Select(processCode, sourceFileID, client).ToList();
            return x;
        }
        public bool SaveBulkSqlInsert(List<Entity.ImportError> list, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            done = DataAccess.FileValidator_ImportError.SaveBulkSqlInsert(list, client);
            return done;
        }
        public bool SaveBulkSqlInsert(Entity.ImportError x, KMPlatform.Object.ClientConnections client)
        {
            bool done = false;
            done = DataAccess.FileValidator_ImportError.SaveBulkSqlInsert(x, client);
            return done;
        }
    }
}
