using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class RuleSet
    {
        public int Save(Entity.RuleSet x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.RuleSetId = DataAccess.RuleSet.Save(x);
                scope.Complete();
            }
            return x.RuleSetId;
        }
        public HashSet<Entity.RuleSet> GetRuleSetsForClient(int clientId)
        {
            HashSet<Entity.RuleSet> retList = new HashSet<Entity.RuleSet>(DataAccess.RuleSet.GetRuleSetsForClient(clientId).ToList());
            return retList;
        }
        public bool UpdateRuleSet_Name_IsGlobal(int ruleSetId, string ruleSetName, bool isGlobalRuleSet, int updatedByUserId)
        {
            bool result = true;
            using (TransactionScope scope = new TransactionScope())
            {
                result = DataAccess.RuleSet.UpdateRuleSet_Name_IsGlobal(ruleSetId, ruleSetName, isGlobalRuleSet, updatedByUserId);
                scope.Complete();
            }
            return result;
        }
        public bool CopyRuleSet(int existingRuleSetId, int newRuleSetId, int createdByUserId)
        {
            bool result = true;
            using (TransactionScope scope = new TransactionScope())
            {
                result = DataAccess.RuleSet.CopyRuleSet(existingRuleSetId, newRuleSetId, createdByUserId);
                scope.Complete();
            }
            return result;
        }
        public Entity.RuleSet GetSourceFile(int sourceFileId, bool isActive = true)
        {
            Entity.RuleSet rs = new Entity.RuleSet();
            rs = DataAccess.RuleSet.GetSourceFile(sourceFileId, isActive);
            return rs;
        }
        public Entity.RuleSet GetRuleSetName(string ruleSetName, int clientID)
        {
            Entity.RuleSet rs = new Entity.RuleSet();
            rs = DataAccess.RuleSet.GetRuleSetName(ruleSetName, clientID);
            return rs;
        }
        public bool RuleSetNameExists(string ruleSetName, int clientID)
        {
            bool result = true;
            using (TransactionScope scope = new TransactionScope())
            {
                result = DataAccess.RuleSet.RuleSetNameExists(ruleSetName, clientID);
                scope.Complete();
            }
            return result;
        }

        #region Object.RuleSet
        public Object.RuleSet GetRuleSetObject(int ruleSetId)
        {
            Object.RuleSet result = null;
            using (TransactionScope scope = new TransactionScope())
            {
                result = DataAccess.RuleSet.GetRuleSetObject(ruleSetId);
                scope.Complete();
            }
            return result;
        }
        /// <summary>
        /// will join RuleSet and RuleSet_File_Map on SourceFileId
        ///     - if the result is NULL - this means file was setup with system defaults
        ///     - if system defaults then join on FileTypeId
        /// </summary>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        public HashSet<Object.RuleSet> GetRuleSetsForSourceFile(int sourceFileId, bool isActive = true)
        {
            HashSet<Object.RuleSet> retList = new HashSet<Object.RuleSet>(DataAccess.RuleSet.GetRuleSetsForSourceFile(sourceFileId, isActive).ToList());
            BusinessLogic.Rule rWrk = new Rule();
            HashSet<Object.Rule> ruleList = rWrk.GetRulesForSourceFile(sourceFileId, isActive);

            foreach (var obj in retList)
            {
                obj.Rules = new HashSet<Object.Rule>(ruleList.Where(x => x.RuleSetId == obj.RuleSetId && x.IsActive == isActive).ToList());
                //could I call this once by sourceFileId - get the list of Rules/RuleValues for the RuleSet then just loop through and apply - that way only 1 db call
                //obj.Rules = rWrk.GetRulesForRuleSet(obj.RuleSetId,isActive);
            }
            return retList;
        }
        public HashSet<Object.RuleSet> GetRuleSetsForClient(int clientId, bool isActive = true)
        {
            HashSet<Object.RuleSet> retList = new HashSet<Object.RuleSet>(DataAccess.RuleSet.GetRuleSetsForSourceFile(clientId, isActive).ToList());
            BusinessLogic.Rule rWrk = new Rule();
            HashSet<Object.Rule> ruleList = rWrk.GetRulesForClient(clientId, isActive);//rWrk.GetRulesForSourceFile(obj.SourceFileId, isActive);
            foreach (var obj in retList)
            {
                obj.Rules = new HashSet<Object.Rule>(ruleList.Where(x => x.RuleSetId == obj.RuleSetId && x.IsActive == isActive).ToList());
                //could I call this once by sourceFileId - get the list of Rules/RuleValues for the RuleSet then just loop through and apply - that way only 1 db call
                //obj.Rules = rWrk.GetRulesForRuleSet(obj.RuleSetId,isActive);
            }
            return retList;
        }
        /// <summary>
        /// will join RuleSet and RuleSet_File_Map on SourceFileId
        ///     - if the result is NULL - this means file was setup with system defaults
        ///     - if system defaults then join on FileTypeId
        /// </summary>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        public HashSet<Object.RuleSet> GetRuleSetsForSourceFile(int sourceFileId, FrameworkUAD_Lookup.Enums.ExecutionPointType executionPoint, bool isActive = true)
        {
            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            int executionPointId = 0;
            FrameworkUAD_Lookup.Entity.Code epCode = cWrk.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.Execution_Points, executionPoint.ToString());
            if (epCode != null && epCode.CodeId > 0)
                executionPointId = epCode.CodeId;
            HashSet<Object.RuleSet> retList = new HashSet<Object.RuleSet>(DataAccess.RuleSet.GetRuleSetsForSourceFile(sourceFileId, executionPointId, isActive).ToList());

            BusinessLogic.Rule rWrk = new Rule();
            foreach (var obj in retList)
            {
                obj.Rules = rWrk.GetRulesForRuleSet(obj.RuleSetId, isActive);
            }
            return retList;
        }
        /// <summary>
        /// will join RuleSet and RuleSet_File_Map on FileTypeId
        ///     - returns all system ruleSets for each FileType
        ///     - FileTypes are: ACS,Other,Audience_Data,NCOA,Telemarketing_Long_Form,Telemarketing_Short_Form,Complimentary,Data_Compare,Web_Forms,List_Source_2YR,
        ///                      List_Source_3YR,List_Source_Other,Field_Update,QuickFill,Paid_Transaction,Web_Forms_Short_Form 
        /// </summary>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        public HashSet<Object.RuleSet> GetSystemRuleSets(bool isActive = true)
        {
            HashSet<Object.RuleSet> retList = new HashSet<Object.RuleSet>(DataAccess.RuleSet.GetSystemRuleSets(isActive).ToList());
            BusinessLogic.Rule rWrk = new Rule();
            foreach (var obj in retList)
            {
                obj.Rules = rWrk.GetSystemRulesForRuleSet(obj.RuleSetId, isActive);
            }
            return retList;
        }
        #endregion


        #region FileMappingWizard

        #endregion
    }
}
