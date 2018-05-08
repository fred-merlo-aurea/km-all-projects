using FrameworkUAD.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ProductTypes
    {
        public FrameworkUAD.Object.Enums.Entity entity = FrameworkUAD.Object.Enums.Entity.ProductType;

        public bool ExistsByIDPubTypeDisplayName(int PubTypeID, string PubTypeDisplayName, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.ProductTypes.ExistsByIDPubTypeDisplayName(PubTypeID, PubTypeDisplayName, client);
            return exists;
        }

        public List<Entity.ProductTypes> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductTypes> x = null;
            x = DataAccess.ProductTypes.Select(client).ToList();
            return x;
        }

        public List<Entity.ProductTypes> SelectByBrand(int BrandID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductTypes> x = null;
            x = DataAccess.ProductTypes.SelectByBrand(BrandID,client).ToList();
            return x;
        }

        public Entity.ProductTypes SelectByID(int PubTypeID, KMPlatform.Object.ClientConnections client)
        {
            Entity.ProductTypes x = DataAccess.ProductTypes.SelectByID(PubTypeID, client);
            return x;
        }

        public int Save(Entity.ProductTypes x, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Save;
            List<UADError> errorList = new List<UADError>();

            if (!x.IsActive && x.PubTypeID > 0)
            {
                if (new FrameworkUAD.BusinessLogic.Product().ExistsByPubTypeID(x.PubTypeID, client))
                {
                    errorList.Add(new UADError(entity, method, "Publication Type cannot be deleted. There is a pub associated with the Publication Type."));
                    throw new UADException(errorList);
                }
            }

            if (ExistsByIDPubTypeDisplayName(x.PubTypeID, x.PubTypeDisplayName, client))
            {
                errorList.Add(new UADError(entity, method, "Display Name already exists."));
                throw new UADException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                x.PubTypeID = DataAccess.ProductTypes.Save(x, client);
                scope.Complete();
            }

            return x.PubTypeID;
        }

        public bool Delete(KMPlatform.Object.ClientConnections client, int PubTypeID)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Delete;
            List<UADError> errorList = new List<UADError>();
            bool delete = false;

            if (new FrameworkUAD.BusinessLogic.Product().ExistsByPubTypeID(PubTypeID, client))
            {
                errorList.Add(new UADError(entity, method, "Publication Type cannot be successfully deleted. There is a pub associated with the Publication Type."));
                throw new UADException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.ProductTypes.Delete(client, PubTypeID);
                scope.Complete();
            }

            return delete;
        }
    }
}
