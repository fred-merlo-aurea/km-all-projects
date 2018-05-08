using FrameworkUAD.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

namespace FrameworkUAD.BusinessLogic
{
    public class MasterCodeSheet
    {
        public FrameworkUAD.Object.Enums.Entity entity = FrameworkUAD.Object.Enums.Entity.MasterCodeSheet;

        public bool ExistsByIDMasterValueMasterGroupID(int masterID, int masterGroupID, string masterValue, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.MasterCodeSheet.ExistsByIDMasterValueMasterGroupID(masterID, masterGroupID, masterValue, client);
            return exists;
        }

        public List<Entity.MasterCodeSheet> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.MasterCodeSheet> x = null;
            x = DataAccess.MasterCodeSheet.Select(client).ToList();
            return x;
        }

        public Entity.MasterCodeSheet SelectByID(int masterID, KMPlatform.Object.ClientConnections client)
        {
            Entity.MasterCodeSheet x = DataAccess.MasterCodeSheet.SelectByID(masterID, client);
            return x;
        }

        public List<Entity.MasterCodeSheet> SelectMasterGroupID(KMPlatform.Object.ClientConnections client, int masterGroupID)
        {
            List<Entity.MasterCodeSheet> x = null;
            x = DataAccess.MasterCodeSheet.SelectMasterGroupID(client, masterGroupID).ToList();
            return x;
        }
        public List<Entity.MasterCodeSheet> SelectMasterBrandID(KMPlatform.Object.ClientConnections client, int brandID)
        {
            List<Entity.MasterCodeSheet> x = null;
            x = DataAccess.MasterCodeSheet.SelectMasterGroupBrandID(client, brandID).ToList();
            return x;
        }

        public DataSet SelectByCodeSheetID(int codeSheetID, KMPlatform.Object.ClientConnections client)
        {
            DataSet x = null;
            x = DataAccess.MasterCodeSheet.SelectByCodeSheetID(codeSheetID, client);
            return x;
        }

        public int ImportSubscriber(int masterID, XDocument xDoc, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                masterID = DataAccess.MasterCodeSheet.ImportSubscriber(masterID, xDoc, client);
                scope.Complete();
            }

            return masterID;
        }

        public DataSet SelectByMasterCodeSheetSearch(int masterGroupID, string name, string searchCriteria, int currentPage, int pageSize, string sortDirection, string sortColumn, KMPlatform.Object.ClientConnections client)
        {
            DataSet x = null;
            x = DataAccess.MasterCodeSheet.SelectByMasterCodeSheetSearch(masterGroupID, name, searchCriteria, currentPage, pageSize, sortDirection, sortColumn, client);
            return x;
        }

        public void UpdateSortOrder(string mastercodesheetXml, KMPlatform.Object.ClientConnections client)
        {
            DataAccess.MasterCodeSheet.UpdateSort(mastercodesheetXml, client);
        }

        public int Save(Entity.MasterCodeSheet x, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Save;
            List<UADError> errorList = new List<UADError>();

            if (ExistsByIDMasterValueMasterGroupID(x.MasterID, x.MasterGroupID, x.MasterValue, client))
            {
                errorList.Add(new UADError(entity, method, "Value already exists."));
                throw new UADException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                x.MasterID = DataAccess.MasterCodeSheet.Save(x, client);
                scope.Complete();
            }

            return x.MasterID;
        }
        public bool DeleteMasterID(KMPlatform.Object.ClientConnections client, int masterID)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.MasterCodeSheet.DeleteMasterID(client, masterID);
                scope.Complete();
            }

            return delete;
        }
        public void CreateNoValueRespones(KMPlatform.Object.ClientConnections client)
        {
            DataAccess.MasterCodeSheet.CreateNoValueRespones(client);
        }
    }
}
