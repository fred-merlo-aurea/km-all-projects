using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ProductSubscriptionDetail
    {
        public List<Entity.ProductSubscriptionDetail> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductSubscriptionDetail> x = null;
            x = DataAccess.ProductSubscriptionDetail.Select(client).ToList();
            return x;
        }
        public List<Entity.ProductSubscriptionDetail> Select(int pubSubscriptionID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductSubscriptionDetail> retList = null;
            retList = DataAccess.ProductSubscriptionDetail.Select(pubSubscriptionID, client);

            return retList;
        }
        public bool Delete(KMPlatform.Object.ClientConnections client, int codeSheetID)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.ProductSubscriptionDetail.DeleteCodeSheetID(client, codeSheetID);
                scope.Complete();
            }

            return delete;
        }

        public List<Entity.ProductSubscriptionDetail> SelectPaging(int page, int pageSize, int productID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductSubscriptionDetail> x = null;
            x = DataAccess.ProductSubscriptionDetail.SelectPaging(page, pageSize, productID, client);
            return x;
        }

        public int SelectCount(int productID, KMPlatform.Object.ClientConnections client)
        {
            int x = 0;
            x = DataAccess.ProductSubscriptionDetail.SelectCount(productID, client);
            return x;
        }
        public bool Save(KMPlatform.Object.ClientConnections client, Entity.ProductSubscriptionDetail x)
        {
            bool saveDone = false;
            using (TransactionScope scope = new TransactionScope())
            {
                saveDone = DataAccess.ProductSubscriptionDetail.Save(client, x);
                scope.Complete();
            }

            return saveDone;
        }
        
        public List<FrameworkUAD.Entity.ProductSubscriptionDetail> ProductSubscriptionDetailUpdateBulkSql(KMPlatform.Object.ClientConnections client, List<Entity.ProductSubscriptionDetail> list)
        {
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;

            List<Entity.ProductSubscriptionDetail> returnList = new List<Entity.ProductSubscriptionDetail>();
            List<Entity.ProductSubscriptionDetail> bulkUpdateList = new List<Entity.ProductSubscriptionDetail>();
            foreach (Entity.ProductSubscriptionDetail x in list)
            {
                counter++;
                processedCount++;
                bulkUpdateList.Add(x);
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            returnList.AddRange(DataAccess.ProductSubscriptionDetail.ProductSubscriptionDetailUpdateBulkSql(client, bulkUpdateList));

                            scope.Complete();
                        }
                        catch (Exception)
                        {
                            scope.Dispose();
                        }
                    }
                    counter = 0;
                    bulkUpdateList = new List<Entity.ProductSubscriptionDetail>();
                }
            }

            return returnList;
        }
    }
}
