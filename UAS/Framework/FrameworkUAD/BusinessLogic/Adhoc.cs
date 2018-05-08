using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class Adhoc
    {
        public List<Entity.Adhoc> SelectAll(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.Adhoc> x = null;
            x = DataAccess.Adhoc.SelectAll(client).ToList();
            return x;
        }
        public List<Entity.Adhoc> SelectCategoryID(int categoryID, KMPlatform.Object.ClientConnections client, int brandID = 0, int pubID = 0)
        {
            List<Entity.Adhoc> x = null;
            x = DataAccess.Adhoc.SelectCategoryID(categoryID, brandID, pubID, client).ToList();
            return x;
        }
        public List<Entity.Adhoc> GetByCategoryID(int categoryID, KMPlatform.Object.ClientConnections client, int brandID = 0, int pubID = 0)
        {
            List<Entity.Adhoc> x = null;
            x = DataAccess.Adhoc.GetByCategoryID(categoryID, brandID, pubID, client).ToList();
            return x;
        }
        public int Save(Entity.Adhoc x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.AdhocID = DataAccess.Adhoc.Save(x, client);
                scope.Complete();
            }

            return x.AdhocID;
        }

        public bool Delete(int categoryID, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.Adhoc.Delete(categoryID, client);
        }

        public bool Delete_AdHoc(int adhocID, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.Adhoc.Delete_AdHoc(adhocID, client);
        }
    }
}
