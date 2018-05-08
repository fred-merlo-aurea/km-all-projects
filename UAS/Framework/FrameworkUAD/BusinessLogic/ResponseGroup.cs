using FrameworkUAD.Object;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ResponseGroup
    {
        public FrameworkUAD.Object.Enums.Entity entity = FrameworkUAD.Object.Enums.Entity.ResponseGroup;

        public bool ExistsByName(string name, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.ResponseGroup.ExistsByName(name, client);
            return exists;
        }

        public bool ExistsByIDDisplayNamePubID(int pubID, int responseGroupID, string displayName, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.ResponseGroup.ExistsByIDDisplayNamePubID(pubID, responseGroupID, displayName, client);
            return exists;
        }

        public bool ExistsByIDResponseGroupNamePubID(int pubID, int responseGroupID, string responseGroupName, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.ResponseGroup.ExistsByIDResponseGroupNamePubID(pubID, responseGroupID, responseGroupName, client);
            return exists;
        }

        public NameValueCollection ValidationForDeleteorInActive(int pubID, int responseGroupID, KMPlatform.Object.ClientConnections client)
        {
            NameValueCollection nvReturn = new NameValueCollection();
            nvReturn = DataAccess.ResponseGroup.ValidationForDeleteorInActive(pubID, responseGroupID, client);
            return nvReturn;
        }

        public List<Entity.ResponseGroup> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ResponseGroup> x = null;
            x = DataAccess.ResponseGroup.Select(client).ToList();
            return x;
        }

        public List<Entity.ResponseGroup> Select(int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ResponseGroup> x = null;
            x = DataAccess.ResponseGroup.Select(pubID, client);
            return x;
        }

        public Entity.ResponseGroup SelectByID(int responseGroupID, KMPlatform.Object.ClientConnections client)
        {
            Entity.ResponseGroup x = DataAccess.ResponseGroup.SelectByID(responseGroupID, client);
            return x;
        }

        public DataSet SelectByResponseGroupsSearch(int pubID, string name, string searchCriteria, int currentPage, int pageSize, string sortDirection, string sortColumn, KMPlatform.Object.ClientConnections client)
        {
            DataSet x = null;
            x = DataAccess.ResponseGroup.SelectByResponseGroupsSearch(pubID, name, searchCriteria, currentPage, pageSize, sortDirection, sortColumn, client);
            return x;
        }

        public int Save(Entity.ResponseGroup x, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Save;
            List<UADError> errorList = new List<UADError>();

            if (!x.IsActive ?? false && x.ResponseGroupID > 0)
            {
                NameValueCollection nvc = ValidationForDeleteorInActive(x.PubID, x.ResponseGroupID, client);

                if (nvc.Count > 0)
                {
                    string errorMsg = "The selected responsegroup is being used in the following scheduled exports, filters or Download Templates and cannot be updated as inactive until all prior uses are previously deleted.</br></br>";

                    var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                    foreach (var item in items)
                    {
                        errorMsg += item.key + " " + item.value + "</br>";
                    }

                    errorList.Add(new UADError(entity, method, errorMsg));
                    throw new UADException(errorList);
                }
            }

            if (ExistsByIDDisplayNamePubID(x.PubID, x.ResponseGroupID, x.DisplayName, client))
            {
                errorList.Add(new UADError(entity, method, "Display Name already exists."));
            }

            if (ExistsByIDResponseGroupNamePubID(x.PubID, x.ResponseGroupID, x.ResponseGroupName, client))
            {
                errorList.Add(new UADError(entity, method, "Name already exists."));
            }

            if (new FrameworkUAD.BusinessLogic.Subscription().ExistsStandardFieldName(x.DisplayName, client))
            {
                errorList.Add(new UADError(entity, method, "Display Name cannot be the same as a Standard Field name. Please enter a different display Display Name."));
            }

            if (new FrameworkUAD.BusinessLogic.Subscription().ExistsStandardFieldName(x.ResponseGroupName, client))
            {
                errorList.Add(new UADError(entity, method, "Name cannot be the same as a Standard Field name. Please enter a different Name."));
            }

            if (new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper().ExistsByCustomField(x.DisplayName, client))
            {
                errorList.Add(new UADError(entity, method, "Display Name cannot be the same as a Product Custom Field. Please enter a different Display Name."));
            }

            if (new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper().ExistsByCustomField(x.DisplayName, client))
            {
                errorList.Add(new UADError(entity, method, "Name cannot be the same as a Product Custom Field. Please enter a different Name."));
            }

            if (errorList.Count > 0)
            {
                throw new UADException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                x.PubID = DataAccess.ResponseGroup.Save(x, client);
                scope.Complete();
            }

            return x.PubID;
        }

        public bool Copy(KMPlatform.Object.ClientConnections client, int responseGroupID, string destPubsXML)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.ResponseGroup.Copy(client, responseGroupID, destPubsXML);
                scope.Complete();
            }

            return delete;
        }

        public bool Delete(KMPlatform.Object.ClientConnections client, int responseGroupID, int pubID)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Delete;
            List<UADError> errorList = new List<UADError>();

            NameValueCollection nvc = ValidationForDeleteorInActive(pubID, responseGroupID, client);

            if (nvc.Count > 0)
            {
                string errorMsg = "The selected responsegroup is being used in the following scheduled exports, filters or Download Templates and cannot be successfully deleted until all prior uses are previously deleted.</br></br>";

                var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                foreach (var item in items)
                {
                    errorMsg += item.key + " " + item.value + "</br>";
                }

                errorList.Add(new UADError(entity, method, errorMsg));
                throw new UADException(errorList);
            }

            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.ResponseGroup.Delete(client, responseGroupID);
                scope.Complete();
            }

            return delete;
        }
    }
}
