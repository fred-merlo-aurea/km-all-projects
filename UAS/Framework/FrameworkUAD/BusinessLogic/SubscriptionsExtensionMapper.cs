using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Transactions;
using FrameworkUAD.Object;

namespace FrameworkUAD.BusinessLogic
{
    public class SubscriptionsExtensionMapper
    {
        public FrameworkUAD.Object.Enums.Entity entity = FrameworkUAD.Object.Enums.Entity.CustomField;
        List<UADError> errorList = new List<UADError>();

        public bool ExistsByCustomField(string customField, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.SubscriptionsExtensionMapper.ExistsByCustomField(customField, client);
            return exists;
        }

        public bool ExistsByIDCustomField(int subscriptionsExtensionMapperID, string customField, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.SubscriptionsExtensionMapper.ExistsByIDCustomField(subscriptionsExtensionMapperID, customField, client);
            return exists;
        }

        public List<Entity.SubscriptionsExtensionMapper> SelectAll(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.SubscriptionsExtensionMapper> retList = null;
            retList = DataAccess.SubscriptionsExtensionMapper.SelectAll(client);
            return retList;
        }

        public Entity.SubscriptionsExtensionMapper SelectByID(int subscriptionsExtensionMapperID, KMPlatform.Object.ClientConnections client)
        {
            Entity.SubscriptionsExtensionMapper x = DataAccess.SubscriptionsExtensionMapper.SelectByID(subscriptionsExtensionMapperID, client);
            return x;
        }

        public NameValueCollection ValidationForDeleteorInActive(int subscriptionsExtensionMapperID, KMPlatform.Object.ClientConnections client)
        {
            NameValueCollection nvReturn = new NameValueCollection();
            nvReturn = DataAccess.SubscriptionsExtensionMapper.ValidationForDeleteorInActive(subscriptionsExtensionMapperID, client);
            return nvReturn;
        }

        public int Save(Entity.SubscriptionsExtensionMapper x, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Save;

            if (!x.Active && x.SubscriptionsExtensionMapperID > 0)
            {
                NameValueCollection nvc = ValidationForDeleteorInActive(x.SubscriptionsExtensionMapperID, client);

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

            if (ExistsByIDCustomField(x.SubscriptionsExtensionMapperID, x.CustomField, client))
            {
                errorList.Add(new UADError(entity, method, "Custom Field already exists."));
            }

            if (new FrameworkUAD.BusinessLogic.Subscription().ExistsStandardFieldName(x.CustomField, client))
            {
                errorList.Add(new UADError(entity, method, "Custom Field cannot be the same as a Standard Field name. Please enter a different Custom Field."));
            }

            if (new FrameworkUAD.BusinessLogic.MasterGroup().ExistsByName(x.CustomField, client))
            {
                errorList.Add(new UADError(entity, method, "Custom Field cannot be the same as a Master Group. Please enter a different Custom Field."));
            }

            if (errorList.Count > 0)
            {
                throw new UADException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                x.SubscriptionsExtensionMapperID = DataAccess.SubscriptionsExtensionMapper.Save(x, client);
                scope.Complete();
            }

            return x.SubscriptionsExtensionMapperID;
        }

        public void Delete(int subscriptionsExtensionMapperID, KMPlatform.Object.ClientConnections client)
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
                FrameworkUAD.DataAccess.SubscriptionsExtensionMapper.Delete(subscriptionsExtensionMapperID, client);
                scope.Complete();
            }
        }
    }
}
