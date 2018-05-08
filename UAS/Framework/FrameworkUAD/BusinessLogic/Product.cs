using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class Product
    {
        public bool ExistsByPubTypeID(int pubTypeID, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.Product.ExistsByPubTypeID(pubTypeID, client);
            return exists;
        }

        public List<Entity.Product> Select(KMPlatform.Object.ClientConnections client, bool includeCustomProperties = false)
        {
            List<Entity.Product> x = null;
            x = DataAccess.Product.Select(client).ToList();

            if (includeCustomProperties == true)
            {
                foreach (Entity.Product p in x)
                {
                    CustomProperties(client, p);
                }
            }
            return x;
        }
        public List<Entity.Product> SelectByBrandID(KMPlatform.Object.ClientConnections client,int brandID, bool includeCustomProperties = false)
        {
            List<Entity.Product> x = null;
            x = DataAccess.Product.SelectBrandID(client, brandID);
            if (includeCustomProperties == true)
            {
                foreach (Entity.Product p in x)
                {
                    CustomProperties(client, p);
                }
            }
            return x;
        }

        public Entity.Product Select(int pubID, KMPlatform.Object.ClientConnections client, bool includeCustomProperties = false, bool GetLatestData = false)
        {
            Entity.Product x = null;
            x = DataAccess.Product.Select(pubID, client, GetLatestData);
            if (includeCustomProperties == true)
            {
                CustomProperties(client, x);
            }
            return x;
        }
        public Entity.Product Select(string pubCode, KMPlatform.Object.ClientConnections client, bool includeCustomProperties = false)
        {
            Entity.Product x = null;
            x = DataAccess.Product.Select(pubCode, client);
            if (includeCustomProperties == true)
            {
                CustomProperties(client, x);
            }
            return x;
        }

        
        private void CustomProperties(KMPlatform.Object.ClientConnections client, Entity.Product product)
        {
            CodeSheet csWorker = new CodeSheet();
            product.CodeSheets = csWorker.Select(product.PubID, client).ToList();
            ResponseGroup rgWorker = new ResponseGroup();
            product.ResponseGroups = rgWorker.Select(product.PubID, client).ToList();
        }
       
        public int Save(Entity.Product x, KMPlatform.Object.ClientConnections client)
        {
            if (!string.IsNullOrEmpty(x.PubCode))
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    x.PubID = DataAccess.Product.Save(x, client);
                    scope.Complete();
                }

                return x.PubID;
            }
            else
                return 0;
        }

        public bool Copy(KMPlatform.Object.ClientConnections client, int fromID, int toID)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.Product.Copy(client, fromID, toID);
                scope.Complete();
            }

            return delete;
        }

        public bool UpdateLock(KMPlatform.Object.ClientConnections client, int userID)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.Product.UpdateLock(client, userID);
                scope.Complete();
            }

            return delete;
        }
    }
}
