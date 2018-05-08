using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Transactions;
using FrameworkUAD.Object;
using System.Data;

namespace FrameworkUAD.BusinessLogic
{
    public class MasterGroup
    {
        public FrameworkUAD.Object.Enums.Entity entity = FrameworkUAD.Object.Enums.Entity.MasterGroup;

        public bool ExistsByName(string name, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.MasterGroup.ExistsByName(name, client);
            return exists;
        }

        public bool ExistsByIDDisplayName(int masterGroupID, string displayName, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.MasterGroup.ExistsByIDDisplayName(masterGroupID, displayName, client);
            return exists;
        }

        public bool ExistsByIDName(int masterGroupID, string name, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.MasterGroup.ExistsByIDName(masterGroupID, name, client);
            return exists;
        }

        public NameValueCollection ValidationForDeleteorInActive(int masterGroupID, KMPlatform.Object.ClientConnections client)
        {
            NameValueCollection nvReturn = new NameValueCollection();
            nvReturn = DataAccess.MasterGroup.ValidationForDeleteorInActive(masterGroupID, client);
            return nvReturn;
        }

        public List<Entity.MasterGroup> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.MasterGroup> x = null;
            x = DataAccess.MasterGroup.Select(client).ToList();
            return x;
        }

        public List<Entity.MasterGroup> SelectByBrandID(int BrandID,KMPlatform.Object.ClientConnections client)
        {
            List<Entity.MasterGroup> x = null;
            x = DataAccess.MasterGroup.SelectByBrandID(BrandID,client).ToList();
            return x;
        }

        public Entity.MasterGroup SelectByID(int MasterGroupID, KMPlatform.Object.ClientConnections client)
        {
            Entity.MasterGroup x = DataAccess.MasterGroup.SelectByID(MasterGroupID, client);
            return x;
        }

        public DataSet SelectByMasterGroupsSearch(string name, string searchCriteria, int currentPage, int pageSize, string sortDirection, string sortColumn, KMPlatform.Object.ClientConnections client)
        {
            DataSet x = null;
            x = DataAccess.MasterGroup.SelectByMasterGroupsSearch(name, searchCriteria, currentPage, pageSize, sortDirection, sortColumn, client);
            return x;
        }

        public void UpdateSortOrder(string mastergroupXml, KMPlatform.Object.ClientConnections client)
        {
            DataAccess.MasterGroup.UpdateSort(mastergroupXml, client);
        }

        public int Save(Entity.MasterGroup x, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Save;
            List<UADError> errorList = new List<UADError>();

            if (!x.IsActive && x.MasterGroupID > 0)
            {
                NameValueCollection nvc = ValidationForDeleteorInActive(x.MasterGroupID, client);

                if (nvc.Count > 0)
                {
                    string errorMsg = "The selected mastergroup is being used in the following scheduled exports, filters or Download Templates and cannot be updated as inactive until all prior uses are previously deleted.</br></br>";

                    var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                    foreach (var item in items)
                    {
                        errorMsg += item.key + " " + item.value + "</br>";
                    }

                    errorList.Add(new UADError(entity, method, errorMsg));
                    throw new UADException(errorList);
                }
            }

            if (ExistsByIDDisplayName(x.MasterGroupID, x.DisplayName, client))
            {
                errorList.Add(new UADError(entity, method, "Display Name already exists."));
            }

            if (ExistsByIDName(x.MasterGroupID, x.Name, client))
            {
                errorList.Add(new UADError(entity, method, "Name already exists."));
            }

            if (new FrameworkUAD.BusinessLogic.Subscription().ExistsStandardFieldName(x.DisplayName, client))
            {
                errorList.Add(new UADError(entity, method, "Display Name cannot be the same as a Standard Field name. Please enter a different Display Name."));
            }

            if (new FrameworkUAD.BusinessLogic.Subscription().ExistsStandardFieldName(x.Name, client))
            {
                errorList.Add(new UADError(entity, method, "Name cannot be the same as a Standard Field name. Please enter a different Name."));
            }

            if (new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper().ExistsByCustomField(x.DisplayName, client))
            {
                errorList.Add(new UADError(entity, method, "Display Name cannot be the same as a Custom Field. Please enter a different Display Name."));
            }

            if (new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper().ExistsByCustomField(x.DisplayName, client))
            {
                errorList.Add(new UADError(entity, method, "Name cannot be the same as a Custom Field. Please enter a different Name."));
            }

            if (errorList.Count > 0)
            {
                throw new UADException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                x.MasterGroupID = DataAccess.MasterGroup.Save(x, client);
                scope.Complete();
            }

            return x.MasterGroupID;
        }

        public bool Delete(int masterGroupID, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Delete;
            List<UADError> errorList = new List<UADError>();

            NameValueCollection nvc = ValidationForDeleteorInActive(masterGroupID, client);

            if (nvc.Count > 0)
            {
                string errorMsg = "The selected mastergroup is being used in the following scheduled exports or filters and cannot be successfully deleted until all prior uses are previously deleted.</br></br>";

                var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                foreach (var item in items)
                {
                    errorMsg += item.key + " " + item.value + "</br>";
                }

                errorList.Add(new UADError(entity, method, errorMsg));
                throw new UADException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.MasterGroup.Delete(client, masterGroupID);
                scope.Complete();
            }
            return true;
        }
    }
}
