using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class Rule
    {
        public HashSet<Entity.Rule> GetRulesForClient(int clientId)
        {
            HashSet<Entity.Rule> retList = new HashSet<Entity.Rule>(DataAccess.Rule.GetRulesForClient(clientId).ToList());
            //BusinessLogic.RuleValue rvWrk = new RuleValue();
            //Dictionary<int, HashSet<Entity.RuleValue>> dictRV = new Dictionary<int, HashSet<Entity.RuleValue>>();
            //retList.ToList().ForEach(x =>
            //{
            //    if (x.RuleValueId.HasValue == true && !dictRV.ContainsKey(x.RuleValueId.Value))
            //    {
            //        HashSet<Entity.RuleValue> hs = new HashSet<Entity.RuleValue>(distRuleValues.Where(r => r.RuleValueId == x.RuleValueId.Value).ToList());
            //        dictRV.Add(x.RuleValueId.Value, hs);
            //    }
            //});
            //foreach (var obj in retList)
            //{
            //    if (obj.RuleValueId.HasValue)
            //        obj.RuleValues = dictRV.Single(x => x.Key == obj.RuleValueId.Value).Value;
            //}
            return retList;
        }
        public Entity.Rule GetRule(int ruleId)
        {
            Entity.Rule x = DataAccess.Rule.GetRule(ruleId);
            return x;
        }
        public bool IsRuleNameUnique(int clientId, string ruleName)
        {
            bool isUnique = true;
            using (TransactionScope scope = new TransactionScope())
            {
                isUnique = DataAccess.Rule.IsRuleNameUnique(clientId, ruleName);
                scope.Complete();
            }
            return isUnique;
        }
        private static HashSet<Entity.RuleValue> _drv;
        public static HashSet<Entity.RuleValue> distRuleValues
        {
            get
            {
                if (_drv != null)
                    return _drv;
                else
                {
                    BusinessLogic.RuleValue rvWrk = new RuleValue();
                    _drv = rvWrk.SelectIsActive();

                    return _drv;
                }
            }
        }
        public int Save(FrameworkUAS.Entity.Rule rule)
        {
            int ruleId = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                ruleId = DataAccess.Rule.Save(rule);
                scope.Complete();
            }
            return ruleId;
        }
        public int CopyRule(int existingRuleId, int newRuleSetId, int userId)
        {
            int ruleId = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                ruleId = DataAccess.Rule.CopyRule(existingRuleId, newRuleSetId, userId);
                scope.Complete();
            }
            return ruleId;
        }
        #region Object.Rule
        //o_Rule_Select_SourceFileId
        public HashSet<Object.Rule> GetRulesForClient(int clientId, bool isActive = true)
        {
            HashSet<Object.Rule> retList = new HashSet<Object.Rule>(DataAccess.Rule.GetRulesForClient(clientId, isActive).ToList());
            BusinessLogic.RuleValue rvWrk = new RuleValue();

            Dictionary<int, HashSet<Entity.RuleValue>> dictRV = new Dictionary<int, HashSet<Entity.RuleValue>>();
            retList.ToList().ForEach(x =>
            {
                if (x.RuleValueId.HasValue == true && !dictRV.ContainsKey(x.RuleValueId.Value))
                {
                    HashSet<Entity.RuleValue> hs = new HashSet<Entity.RuleValue>(distRuleValues.Where(r => r.RuleValueId == x.RuleValueId.Value).ToList());
                    dictRV.Add(x.RuleValueId.Value, hs);
                }
            });

            //HashSet<Entity.RuleValue> rvHS = rvWrk.GetRuleValuesForSourceFile(sourceFileId, isActive);
            foreach (var obj in retList)
            {
                if (obj.RuleValueId.HasValue)
                    obj.RuleValues = dictRV.Single(x => x.Key == obj.RuleValueId.Value).Value;

                //if (obj.RuleValueId.HasValue)
                //    obj.RuleValues = new HashSet<Entity.RuleValue>(rvHS.ToList().Where(x => x.RuleValueId == obj.RuleValueId.Value).ToList());
                //else
                //    rvWrk.GetRuleValuesForRule(obj.RuleId, isActive);//new HashSet<Entity.RuleValue>(rvHS.ToList().Where(x => x.RuleId == obj.RuleId).ToList());
            }
            return retList;
        }
        public HashSet<Object.Rule> GetRulesForSourceFile(int sourceFileId, bool isActive = true)
        {
            HashSet<Object.Rule> retList = new HashSet<Object.Rule>(DataAccess.Rule.GetRulesForSourceFile(sourceFileId, isActive).ToList());
            BusinessLogic.RuleValue rvWrk = new RuleValue();

            Dictionary<int, HashSet<Entity.RuleValue>> dictRV = new Dictionary<int, HashSet<Entity.RuleValue>>();
            retList.ToList().ForEach(x =>
            {
                if (x.RuleValueId.HasValue == true && !dictRV.ContainsKey(x.RuleValueId.Value))
                {
                    HashSet<Entity.RuleValue> hs = new HashSet<Entity.RuleValue>(distRuleValues.Where(r => r.RuleValueId == x.RuleValueId.Value).ToList());
                    dictRV.Add(x.RuleValueId.Value, hs);
                }
            });

            //HashSet<Entity.RuleValue> rvHS = rvWrk.GetRuleValuesForSourceFile(sourceFileId, isActive);
            foreach (var obj in retList)
            {
                if (obj.RuleValueId.HasValue)
                    obj.RuleValues = dictRV.Single(x => x.Key == obj.RuleValueId.Value).Value;

                //if (obj.RuleValueId.HasValue)
                //    obj.RuleValues = new HashSet<Entity.RuleValue>(rvHS.ToList().Where(x => x.RuleValueId == obj.RuleValueId.Value).ToList());
                //else
                //    rvWrk.GetRuleValuesForRule(obj.RuleId, isActive);//new HashSet<Entity.RuleValue>(rvHS.ToList().Where(x => x.RuleId == obj.RuleId).ToList());
            }
            return retList;
        }
        public HashSet<Object.Rule> GetRulesForRuleSet(int ruleSetId, bool isActive = true)
        {
            HashSet<Object.Rule> retList = new HashSet<Object.Rule>(DataAccess.Rule.GetRulesForRuleSet(ruleSetId, isActive).ToList());

            BusinessLogic.RuleValue rvWrk = new RuleValue();
            foreach (var obj in retList)
            {
                if (obj.RuleValueId.HasValue)
                    obj.RuleValues = rvWrk.GetRuleValuesForRuleValue(obj.RuleValueId.Value, isActive);
                else
                    obj.RuleValues = rvWrk.GetRuleValuesForRule(obj.RuleId, isActive);
            }
            return retList;
        }
        public HashSet<Object.Rule> GetSystemRulesForRuleSet(int ruleSetId, bool isActive = true)
        {
            HashSet<Object.Rule> retList = new HashSet<Object.Rule>(DataAccess.Rule.GetSystemRulesForRuleSet(ruleSetId, isActive).ToList());

            BusinessLogic.RuleValue rvWrk = new RuleValue();
            foreach (var obj in retList)
            {
                if (obj.RuleValueId.HasValue)
                    obj.RuleValues = rvWrk.GetRuleValuesForRuleValue(obj.RuleValueId.Value, isActive);
                else
                    obj.RuleValues = rvWrk.GetRuleValuesForRule(obj.RuleId, isActive);
            }
            return retList;
        }
        public static HashSet<Object.Rule> GetSystemRules(bool isActive = true)
        {
            HashSet<Object.Rule> retList = new HashSet<Object.Rule>(DataAccess.Rule.GetSystemRules(isActive).ToList());

            BusinessLogic.RuleValue rvWrk = new RuleValue();
            foreach (var obj in retList)
            {
                if (obj.RuleValueId.HasValue)
                    obj.RuleValues = rvWrk.GetRuleValuesForRuleValue(obj.RuleValueId.Value, isActive);
                else
                    obj.RuleValues = rvWrk.GetRuleValuesForRule(obj.RuleId, isActive);
            }
            return retList;
        }
        public Object.Rule GetRuleObject(int ruleId)
        {
            Object.Rule obj = DataAccess.Rule.GetRuleObject(ruleId);
            BusinessLogic.RuleValue rvWrk = new RuleValue();
            if (obj.RuleValueId.HasValue)
                obj.RuleValues = rvWrk.GetRuleValuesForRuleValue(obj.RuleValueId.Value);
            else
                obj.RuleValues = rvWrk.GetRuleValuesForRule(obj.RuleId);
            return obj;
        }
        #endregion
        #region Object.CustomRuleGrid
        public List<Object.CustomRule> GetCustomRules(int ruleSetId)
        {
            List<Object.CustomRule> retList = new List<Object.CustomRule>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.Rule.GetCustomRules(ruleSetId).ToList();
                scope.Complete();
            }
            retList.ForEach(m =>
            {
                m.Updates = DataAccess.Rule.GetCustomRuleInsertUpdateNew(m.RuleId);
            });
            return retList;
        }
        public Object.CustomRule GetCustomRule(int ruleId)
        {
            Object.CustomRule obj = DataAccess.Rule.GetCustomRule(ruleId);
            obj.Updates = DataAccess.Rule.GetCustomRuleInsertUpdateNew(ruleId);
            return obj;
        }
        #endregion
    }
}
