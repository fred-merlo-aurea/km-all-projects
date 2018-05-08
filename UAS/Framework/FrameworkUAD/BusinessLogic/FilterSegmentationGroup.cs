using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class FilterSegmentationGroup
    {

        public List<Entity.FilterSegmentationGroup> SelectByFilterSegmentationID(int filterSegmentationID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FilterSegmentationGroup> retList = null;
            retList = DataAccess.FilterSegmentationGroup.SelectByFilterSegmentationID(filterSegmentationID, client);
            return retList;
        }

        public int Save(Entity.FilterSegmentationGroup x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.FilterSegmentationGroupID = DataAccess.FilterSegmentationGroup.Save(x, client);
                scope.Complete();
            }

            return x.FilterSegmentationGroupID;
        }

        // Task 47938:Filter Segmentation - cannot save over an existing Filter Segmenation
        public int DeleteByFilterSegmentationID(int filterSegmentationID, KMPlatform.Object.ClientConnections client)
        {
            var rowsDeleted = DataAccess.FilterSegmentationGroup.DeleteByFilterSegmentationID(filterSegmentationID, client);
            return rowsDeleted;
        }
    }
}
