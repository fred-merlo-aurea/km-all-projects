using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    [Serializable]
    [DataContract]
    public class CustomRuleInsertUpdateNew
    {
        public CustomRuleInsertUpdateNew() { }
        #region properties
        [DataMember]
        public string dataTableColumnName { get; set; }
        [DataMember]
        public string columnName
        {
            get
            {
                if (!string.IsNullOrEmpty(dataTableColumnName) && dataTableColumnName.Contains("."))
                    return dataTableColumnName.Split('.')[1].ToString();
                else
                    return string.Empty;
            }
        }
        [DataMember]
        public string dataTable
        {
            get
            {
                if (!string.IsNullOrEmpty(dataTableColumnName) && dataTableColumnName.Contains("."))
                    return dataTableColumnName.Split('.')[0].ToString();
                else
                    return string.Empty;
            }
        }
        [DataMember]
        public bool isClientField { get; set; }
        [DataMember]
        public string uiControl { get; set; }
        [DataMember]
        public string dataType { get; set; }
        [DataMember]
        public int ruleFieldId { get; set; }
        [DataMember]
        public bool isMultiSelect { get; set; }
        [DataMember]
        public string updateText { get; set; }
        [DataMember]
        public string updateValue { get; set; }
        #endregion
    }
}
