using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAS.Model
{
    [Serializable]
    [DataContract]
    public class RuleSet
    {
        public RuleSet()
        {
            rules = new List<Rule>();
        }
        public RuleSet(int _sourceFileId, bool _isFullFile, string _ruleTypeTab, int _ruleSetId = 0, string _ruleSetName = "", string _description = "", bool getRules = false)
        {
            sourceFileId = _sourceFileId;
            isFullFile = _isFullFile;
            ruleSetId = _ruleSetId;
            ruleSetName = !string.IsNullOrEmpty(_ruleSetName) ? _ruleSetName : "Rule Set - sourceFileId " + _sourceFileId.ToString() + " " + DateTime.Now.ToString("MMddyyyy");
            description = !string.IsNullOrEmpty(_description) ? _description : "new rule set created on " + DateTime.Now.ToString();
            isGlobalRuleSet = false;
            rules = new List<Rule>();
            ruleTypeTab = _ruleTypeTab;

            if (getRules && _sourceFileId > 0 && _ruleSetId > 0)
                rules = new FrameworkUAS.BusinessLogic.Model().RulesGetRuleSet(_ruleSetId, _sourceFileId);
        }

        [DataMember]
        public int sourceFileId { get; set; }
        [DataMember]
        public int ruleSetId { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Rule Set name is required.")]
        [Display(Name = "Rule Set Name:")]
        public string ruleSetName { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public bool isFullFile { get; set; }
        [DataMember]
        public bool isGlobalRuleSet { get; set; }
        [DataMember]
        public string ruleTypeTab { get; set; }//UAS.Rule.CustomImportRuleId (UAD_Lookup.Code / CodeType 'Custom Import Rule') valid types are Insert, Update, Delete

        public List<Rule> rules { get; set; }

        //public List<FrameworkUAS.Object.CustomRuleGrid> rulesViewModel { get; set; }
    }
}
