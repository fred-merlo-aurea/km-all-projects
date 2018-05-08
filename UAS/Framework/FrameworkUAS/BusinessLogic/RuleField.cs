using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class RuleField
    {
        //public List<Entity.RuleField> SelectAll()
        //{
        //    List<Entity.RuleField> retList = new List<Entity.RuleField>();
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        retList = DataAccess.RuleField.SelectAll().ToList();
        //        scope.Complete();
        //    }
        //    return retList;
        //}
        //public Entity.RuleField Select(int ruleFieldId)
        //{
        //    Entity.RuleField x = new Entity.RuleField();
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        x = DataAccess.RuleField.Select(ruleFieldId);
        //        scope.Complete();
        //    }
        //    return x;
        //}
        //public Entity.RuleField Select(int clientId, string field, bool isActive)
        //{
        //    field = Core_AMS.Utilities.StringFunctions.RemoveLineBreaks(field);

        //    Entity.RuleField x = new Entity.RuleField();
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        x = DataAccess.RuleField.Select(clientId, field, isActive);
        //        scope.Complete();
        //    }
        //    return x;
        //}
        //public Entity.RuleField Select(int clientId, string dataTable, string field, bool isActive)
        //{
        //    field = Core_AMS.Utilities.StringFunctions.RemoveLineBreaks(field);

        //    Entity.RuleField x = new Entity.RuleField();
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        x = DataAccess.RuleField.Select(clientId, dataTable, field, isActive);
        //        scope.Complete();
        //    }
        //    return x;
        //}
        //public void CreateForClient(string xml, int clientId)
        //{
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        DataAccess.RuleField.CreateForClient(xml, clientId);
        //        scope.Complete();
        //    }
        //}

        /// <summary>
        /// this will return all ACTIVE fields where clientId = 0 (km standard fields) plus all Adhoc/Demo fields for the clientId passed in
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public List<Object.RuleFieldSelectListItem>  GetDropDownList(int clientId)
        {
            List<Object.RuleFieldSelectListItem> retList = new List<Object.RuleFieldSelectListItem>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.RuleField.GetDropDownList(clientId).ToList();
                scope.Complete();
            }
            return retList;
        }

        //public bool CreateForClient(string xml)
        //{
        //    bool complete = false;
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        complete = DataAccess.RuleField.CreateForClient(xml);
        //        scope.Complete();
        //    }
        //    return complete;
        //}
    }
}
