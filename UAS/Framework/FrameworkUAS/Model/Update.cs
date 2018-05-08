using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAS.Model
{
    [Serializable]
    [DataContract]
    public class Update
    {
        public string MAFField { get; set; }
        public string Values { get; set; }

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
    }
}
