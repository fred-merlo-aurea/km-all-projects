using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class FilterSegmentation
    {
        public List<Entity.FilterSegmentation> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.FilterSegmentation> x = null;
            x = DataAccess.FilterSegmentation.Select(client).ToList();
            return x;
        }

        public Entity.FilterSegmentation SelectByID(int filterSegmentationID, KMPlatform.Object.ClientConnections client, bool getChildren = false)
        {
            Entity.FilterSegmentation x = DataAccess.FilterSegmentation.SelectByID(filterSegmentationID, client);

            if (x != null && getChildren)
            {
                x.FilterSegmentationGroupList = DataAccess.FilterSegmentationGroup.SelectByFilterSegmentationID(filterSegmentationID, client);
            }

            return x;
        }

        public DataTable SelectViewTypeUserID(int @UserID, int @PubID, int @BrandID, string @ViewType, bool @IsAdmin, int FilterCategoryID, string SearchText, string SearchCriteria, KMPlatform.Object.ClientConnections client)
        {
            DataTable dt = null;
            dt = DataAccess.FilterSegmentation.SelectViewTypeUserID(@UserID, @PubID, @BrandID, @ViewType, @IsAdmin, FilterCategoryID, SearchText, SearchCriteria, client);
            return dt;
        }

        public bool ExistsByIDName(int filterSegmentationID, string name, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.FilterSegmentation.ExistsByIDName(filterSegmentationID, name, client);
            return exists;
        }

        public int Save(Entity.FilterSegmentation x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.FilterSegmentationID = DataAccess.FilterSegmentation.Save(x, client);
                scope.Complete();
            }

            return x.FilterSegmentationID;
        }

        public bool Delete(int filterSegmentationID, int userID, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.FilterSegmentation.Delete(filterSegmentationID, userID, client);
        }
    }
}
