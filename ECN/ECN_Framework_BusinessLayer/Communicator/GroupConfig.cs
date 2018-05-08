using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Transactions;
using ECN_Framework_Common.Objects;
using System.Configuration;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class GroupConfig
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.GroupConfig;

        public static List<ECN_Framework_Entities.Communicator.GroupConfig> GetByCustomerID(int CustomerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.GroupConfig> GroupConfig = new List<ECN_Framework_Entities.Communicator.GroupConfig>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                GroupConfig = ECN_Framework_DataLayer.Communicator.GroupConfig.GetByCustomerID(CustomerID);
                scope.Complete();
            }
            return GroupConfig;
        }

        public static void Save(ECN_Framework_Entities.Communicator.GroupConfig GroupConfig, KMPlatform.Entity.User user)
        {
            Validate(GroupConfig);
            using (TransactionScope scope = new TransactionScope())
            {
                GroupConfig.GroupConfigID = ECN_Framework_DataLayer.Communicator.GroupConfig.Save(GroupConfig, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int GroupConfigID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.GroupConfig.Delete(GroupConfigID, user.CustomerID ,user.UserID);
                scope.Complete();
            }
        }

        private static void Validate(ECN_Framework_Entities.Communicator.GroupConfig config)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (!ECN_Framework_Common.Functions.RegexUtilities.IsValidUDFName(config.ShortName))
            {
                errorList.Add(new ECNError(Entity, Method, "Short Name is invalid"));
            }

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }
    }
}
