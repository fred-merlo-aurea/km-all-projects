using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class DataFieldSets
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.DataFieldSets;

        public static bool Exists(int datafieldSetID, int groupID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.DataFieldSets.Exists(datafieldSetID, groupID);
                scope.Complete();
            }
            return exists;
        }

        public static ECN_Framework_Entities.Communicator.DataFieldSets GetByDataFieldsetID(int datafieldSetID, int groupID, bool getChildren)
        {
            ECN_Framework_Entities.Communicator.DataFieldSets dfs = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dfs = ECN_Framework_DataLayer.Communicator.DataFieldSets.GetByDataFieldsetID(datafieldSetID, groupID);
                scope.Complete();
            }

            return dfs;
        }

        public static List<ECN_Framework_Entities.Communicator.DataFieldSets> GetByGroupID(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.DataFieldSets> dfsList = new List<ECN_Framework_Entities.Communicator.DataFieldSets>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dfsList = ECN_Framework_DataLayer.Communicator.DataFieldSets.GetByGroupID(groupID);
                scope.Complete();
            }

            return dfsList;
        }

        public static ECN_Framework_Entities.Communicator.DataFieldSets GetByGroupIDName(int groupID, string name)
        {
            ECN_Framework_Entities.Communicator.DataFieldSets dfs = null;
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            //{
                dfs = ECN_Framework_DataLayer.Communicator.DataFieldSets.GetByGroupIDName(groupID, name);
                //scope.Complete();
            //}

            return dfs;
        }

        public static void Validate(ECN_Framework_Entities.Communicator.DataFieldSets set)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();
            if (set.DataFieldSetID > 0 && (set.GroupID <= 0 || !Exists(set.DataFieldSetID, set.GroupID)))
                errorList.Add(new ECNError(Entity, Method, "DataFieldSetID is invalid"));
            if (set.GroupID <= 0)
                errorList.Add(new ECNError(Entity, Method, "GroupID is invalid"));
            if (set.MultivaluedYN.Trim().ToUpper() != "Y" && set.MultivaluedYN.Trim().ToUpper() != "N")
                errorList.Add(new ECNError(Entity, Method, "MultivaluedYN is invalid"));
            if (set.Name.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "Name cannot be empty"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static int Save(ECN_Framework_Entities.Communicator.DataFieldSets set, KMPlatform.Entity.User user)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Save;
            if (set.DataFieldSetID > 0)
            {
                if (!Exists(set.DataFieldSetID, set.GroupID))
                {
                    List<ECNError> errorList = new List<ECNError>();
                    errorList.Add(new ECNError(Entity, Method, "DataFieldSetID is invalid"));
                    throw new ECNException(errorList);
                }
            }
            Validate(set);

            using (TransactionScope scope = new TransactionScope())
            {
                if (set.DataFieldSetID > 0)
                {
                    set.DataFieldSetID = ECN_Framework_DataLayer.Communicator.DataFieldSets.Update(set);
                }
                else
                {
                    set.DataFieldSetID = ECN_Framework_DataLayer.Communicator.DataFieldSets.Insert(set);
                }
                scope.Complete();
            }
            return set.DataFieldSetID;
        }
    }
}
