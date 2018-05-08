using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.BusinessLogic
{
    public class IssueSplitFilterDetails
    {
        public List<Entity.IssueSplitFilterDetails> SelectFilterID(int FilterID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.IssueSplitFilterDetails> x = DataAccess.IssueSplitFilterDetails.SelectFilterID(FilterID, client);
            return x;
        }
    }
}
