using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FrameworkUAS.Model
{
    [Serializable]
    [DataContract]
    public class Rule
    {
        public Rule()
        {
            isTemplateRule = false;
            conditions = new List<Condition>();
            updates = new List<Update>();
            ruleActions = new List<SelectListItem>();
        }
        public Rule(int _sourceFileId, int _ruleSetId)
        {
            ruleSetId = _ruleSetId;
            sourceFileId = _sourceFileId;
            isTemplateRule = false;

            conditions = new List<Condition>();
            updates = new List<Update>();
            ruleActions = new List<SelectListItem>();
        }
        public Rule(string _ruleName, string _ruleType, string _ruleAction, int _sortOrder, bool _isTemplate, int _ruleSetId, int _sourceFieldId)
        {
            ruleName = _ruleName;
            ruleType = _ruleType;
            ruleAction = _ruleAction;
            sortOrder = _sortOrder;
            isTemplateRule = _isTemplate;
            
            ruleSetId = _ruleSetId;
            sourceFileId = _sourceFieldId;
            
            conditions = new List<Condition>();
            updates = new List<Update>();
            ruleActions = new List<SelectListItem>();
        }
        public Rule(int _ruleId, string _ruleName, string _ruleType, string _ruleAction, int _sortOrder, bool _isTemplate, int _ruleSetId, int _sourceFieldId)
        {
            ruleId = _ruleId;
            ruleName = _ruleName;
            ruleType = _ruleType;
            ruleAction = _ruleAction;
            sortOrder = _sortOrder;
            isTemplateRule = _isTemplate;

            ruleSetId = _ruleSetId;
            sourceFileId = _sourceFieldId;

            conditions = new List<Condition>();
            updates = new List<Update>();
            ruleActions = new List<SelectListItem>();
        }
        //RulesPostDQMViewModel( RuleType, SourceFileId, RuleSetName, string.Empty, fmwModel.rulesViewModel.SelectedRuleSetId, 0);



        [DataMember]
        public int ruleId { get; set; }// UAS.Rule
        [DataMember]
        public string ruleName { get; set; }// UAS.Rule
        [DataMember]
        public string ruleType { get; set; }//UAS.Rule.CustomImportRuleId (UAD_Lookup.Code / CodeType 'Custom Import Rule') valid types are Insert, Update, Delete, ADMS
        [DataMember]
        public string ruleAction { get; set; }//UAS.Rule.RuleActionId   valid types are Do Not Import, Import, Update New, Delete, Delete All, Update Existing and File, Update Existing, Update File, Update Existing All, Update File All, Update All
        [DataMember]
        public int sortOrder { get; set; }//UAS.RuleSetRuleOrder.ExecutionOrder 
        [DataMember]
        public bool isTemplateRule { get; set; }//UAS.Rule.IsGlobal
        [DataMember]
        public string ruleScript { get; set; }//UAS.RuleSetRuleOrder.RuleScript 

        //items from RuleSet object
        [DataMember]
        public int ruleSetId { get; set; }//UAS.RuleSetRuleOrder
        [DataMember]
        public int sourceFileId { get; set; }//UAS.RuleSet_File_Map --joining on UAS.RuleSetRuleOrder.RuleSetId

        public List<Condition> conditions { get; set; }//RuleCondtion
        public List<Update> updates { get; set; }//RuleResult

        //holders for dropdowns
        public List<SelectListItem> ruleActions { get; set; }
    }
}
