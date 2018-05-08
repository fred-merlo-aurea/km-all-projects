using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class RuleFieldPredefinedValue
    {
        //public List<Entity.RuleFieldPredefinedValue> SelectAll()
        //{
        //    List<Entity.RuleFieldPredefinedValue> retList = new List<Entity.RuleFieldPredefinedValue>();
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        retList = DataAccess.RuleFieldPredefinedValue.SelectAll().ToList();
        //        scope.Complete();
        //    }
        //    return retList;
        //}
        //public List<Entity.RuleFieldPredefinedValue> Select(int ruleFieldId)
        //{
        //    List<Entity.RuleFieldPredefinedValue> retList = new List<Entity.RuleFieldPredefinedValue>();
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        retList = DataAccess.RuleFieldPredefinedValue.Select(ruleFieldId).ToList();
        //        scope.Complete();
        //    }
        //    return retList;
        //}
        //public List<Entity.RuleFieldPredefinedValue> CreateForClient(string xml, int clientId, KMPlatform.Object.ClientConnections clientCon)
        //{
        //    List<Entity.RuleFieldPredefinedValue> retList = new List<Entity.RuleFieldPredefinedValue>();
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        retList = DataAccess.RuleFieldPredefinedValue.CreateForClient(xml, clientId, clientCon);
        //        scope.Complete();
        //    }
        //    return retList;
        //}
        //public bool SaveBulkSqlInsert(List<Entity.RuleFieldPredefinedValue> list)
        //{
        //    bool done = false;
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        DataAccess.RuleFieldPredefinedValue.SaveBulkSqlInsert(list);
        //        scope.Complete();
        //        done = true;
        //    }
        //    return done;
        //}



        public List<Object.RuleFieldNeedValue> GetRuleFieldsNeedingValues()
        {
            List<Object.RuleFieldNeedValue> retList = new List<Object.RuleFieldNeedValue>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.RuleFieldPredefinedValue.GetRuleFieldsNeedingValues().ToList();
                scope.Complete();
            }
            return retList;
        }
        
    }
}
