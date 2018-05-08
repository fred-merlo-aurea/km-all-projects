using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FrameworkUAS.Model
{
    /// <summary>
    /// items that will be combined to create the where clause 
    /// </summary>
    [Serializable]
    [DataContract]
    public class Condition
    {
        public Condition()
        {

        }
        public Condition(string _comparator, string _mafField, FrameworkUAD_Lookup.Enums.FieldType _fieldType, FrameworkUAD_Lookup.Enums.FieldDataType _fieldDataType,
                         string _operator, string _operatorFunction, string _values, int _lineNumber, int _ruleId, bool _isGrouped, Entity.RuleCondition _ruleCondition)
        {
            connector = _comparator;
            mafField = _mafField;
            fieldType = _fieldType;
            fieldDataType = _fieldDataType;
            Operator = _operator;
            OperatorFunction = _operatorFunction;
            values = _values;
            lineNumber = _lineNumber;
            ruleId = _ruleId;
            isGrouped = _isGrouped;
            ruleCondition = _ruleCondition;

            operators = FrameworkUAD_Lookup.Model.Operator.GetOperators();
            databaseFields = new List<Field>();
            mappedFields = new List<Field>();
        }


        //Items needed to save RuleCondition
        [DataMember]
        public string connector { get; set; }//  and - or    RuleCondition.ChainId
        [DataMember]
        public string mafField { get; set; }//RuleCondition.CompareField
        [DataMember]
        public FrameworkUAD_Lookup.Enums.FieldType fieldType { get; set; }//Profile,Demo,Custom,Lookup_State,Lookup_Country,Lookup_Code,Lookup_Category,Lookup_Transaction
        [DataMember]
        public FrameworkUAD_Lookup.Enums.FieldDataType fieldDataType { get; set; }//Int,String,Date,Datetime,Time,Float,Decimal,Bit,Lookup,Demo
        [DataMember]
        public string Operator { get; set; }//RuleCondition.OperatorId
        [DataMember]
        public string OperatorFunction { get; set; }//??????
        [DataMember]
        public string values { get; set; }//RuleCondition.CompareValue
        [DataMember]
        public int lineNumber { get; set; }//RuleCondition.Line       
        [DataMember]
        public int ruleId { get; set; }//RuleCondition.RuleId
        [DataMember]
        public bool isGrouped { get; set; }//RuleCondition.IsGrouped


        [DataMember]
        public FrameworkUAS.Entity.RuleCondition ruleCondition { get; set; }

        public List<FrameworkUAD_Lookup.Model.Operator> operators { get; set; }//new FrameworkUAD_Lookup.BusinessLogic.Code().GetOperators()
        public List<SelectListItem> connectors
        {
            get
            {
                List<SelectListItem> c = new List<SelectListItem>();
                c.Add(new SelectListItem() { Text = string.Empty, Value = string.Empty });
                c.Add(new SelectListItem() { Text = "And", Value = "And" });
                c.Add(new SelectListItem() { Text = "Or", Value = "Or" });
                return c;
            }
        }
        public IEnumerable<SelectListItem> lookupData { get; set; }//based on fieldType this will hold the pertinent lookup data - state/country/code/cate/tran
        public List<Field> databaseFields { get; set; }//all fields that can be mapped, if Circ filter by PubCode
        public List<Field> mappedFields { get; set; }//fields that actually mapped - not set to Ignore
    }
}
