using FrameworkUAD.Object;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class ProductSubscriptionsExtensionMapper
    {
        public FrameworkUAD.Object.Enums.Entity entity = FrameworkUAD.Object.Enums.Entity.PubCustomField;
        List<UADError> errorList = new List<UADError>();
        public bool ExistsByCustomField(string customField, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.ProductSubscriptionsExtensionMapper.ExistsByCustomField(customField, client);
            return exists;
        }
        public bool ExistsByIDCustomField(int pubSubscriptionsExtensionMapperID, int pubID, string customField, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.ProductSubscriptionsExtensionMapper.ExistsByIDCustomField(pubSubscriptionsExtensionMapperID, pubID, customField, client);
            return exists;
        }
        public List<Entity.ProductSubscriptionsExtensionMapper> SelectAll(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.ProductSubscriptionsExtensionMapper> retList = null;
            retList = DataAccess.ProductSubscriptionsExtensionMapper.SelectAll(client);
            return retList;
        }
        public Entity.ProductSubscriptionsExtensionMapper SelectByID(int pubSubscriptionsExtensionMapperID, KMPlatform.Object.ClientConnections client)
        {
            Entity.ProductSubscriptionsExtensionMapper x = DataAccess.ProductSubscriptionsExtensionMapper.SelectByID(pubSubscriptionsExtensionMapperID, client);
            return x;
        }
        public DataSet SelectBySearch(int pubID, string name, string searchCriteria, int currentPage, int pageSize, string sortDirection, string sortColumn, KMPlatform.Object.ClientConnections client)
        {
            DataSet x = null;
            x = DataAccess.ProductSubscriptionsExtensionMapper.SelectBySearch(pubID, name, searchCriteria, currentPage, pageSize, sortDirection, sortColumn, client);
            return x;
        }
        public NameValueCollection ValidationForDeleteorInActive(int pubSubscriptionsExtensionMapperID, KMPlatform.Object.ClientConnections client)
        {
            NameValueCollection nvReturn = new NameValueCollection();
            nvReturn = DataAccess.ProductSubscriptionsExtensionMapper.ValidationForDeleteorInActive(pubSubscriptionsExtensionMapperID, client);
            return nvReturn;
        }
        public int Save(Entity.ProductSubscriptionsExtensionMapper x, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Save;

            if (!x.Active && x.PubSubscriptionsExtensionMapperID > 0)
            {
                NameValueCollection nvc = ValidationForDeleteorInActive(x.PubSubscriptionsExtensionMapperID, client);

                if (nvc.Count > 0)
                {
                    string errorMsg = "The selected Custom Field is being used in the following scheduled exports or filters and cannot be updated as inactive until all prior uses are previously deleted.</br></br>";

                    var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                    foreach (var item in items)
                    {
                        errorMsg += item.key + " " + item.value + "</br>";
                    }

                    errorList.Add(new UADError(entity, method, errorMsg));
                    throw new UADException(errorList);
                }
            }

            if (ExistsByIDCustomField(x.PubSubscriptionsExtensionMapperID, x.PubID, x.CustomField, client))
            {
                errorList.Add(new UADError(entity, method, "Custom Field already exists."));
            }

            if (new FrameworkUAD.BusinessLogic.Subscription().ExistsStandardFieldName(x.CustomField, client))
            {
                errorList.Add(new UADError(entity, method, "Custom Field cannot be the same as a Standard Field name. Please enter a different Custom Field."));
            }

            if (new FrameworkUAD.BusinessLogic.ResponseGroup().ExistsByName(x.CustomField, client))
            {
                errorList.Add(new UADError(entity, method, "Custom Field cannot be the same as a Response Group. Please enter a different Custom Field."));
            }

            if (errorList.Count > 0)
            {
                throw new UADException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                x.PubSubscriptionsExtensionMapperID = DataAccess.ProductSubscriptionsExtensionMapper.Save(x, client);
                scope.Complete();
            }

            return x.PubSubscriptionsExtensionMapperID;
        }
        public void Delete(int subscriptionsExtensionMapperID, int pubID, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Delete;

            NameValueCollection nvc = ValidationForDeleteorInActive(subscriptionsExtensionMapperID, client);

            if (nvc.Count > 0)
            {
                string errorMsg = "The selected Custom Field is being used in the following scheduled exports or filters and cannot be successfully deleted until all prior uses are previously deleted.</br>";

                var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                foreach (var item in items)
                {
                    errorMsg += item.key + " " + item.value + " </br> ";
                }

                errorList.Add(new UADError(entity, method, errorMsg));
                throw new UADException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                FrameworkUAD.DataAccess.ProductSubscriptionsExtensionMapper.Delete(subscriptionsExtensionMapperID, pubID, client);
                scope.Complete();
            }
        }
    }
}
