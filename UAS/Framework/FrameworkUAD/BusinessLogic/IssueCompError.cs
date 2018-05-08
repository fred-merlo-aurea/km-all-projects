using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class IssueCompError
    {
        public List<Entity.IssueCompError> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueCompError> x = DataAccess.IssueCompError.Select(client);
            return x;
        }

        public List<Entity.IssueCompError> SelectProcessCode(string processCode, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueCompError> x = DataAccess.IssueCompError.SelectProcessCode(processCode, client);
            return x;
        }
    }
}
