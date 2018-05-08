using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FrameworkUAS.Object
{
    [Serializable]
    public class RuleFieldSelectListItem //: SelectListItem
    {
        public RuleFieldSelectListItem()
        {
            RuleFieldId = 0;
            DataTable = string.Empty;
            Field = string.Empty;
            UIControl = "textbox";
            IsMultiSelect = false;
            
            //Item = new SelectListItem();
        }
        public RuleFieldSelectListItem(int ruleFieldId, string dataTable, string field, string uiControl, bool isMultiSelect)
        {
            RuleFieldId = ruleFieldId;
            DataTable = dataTable;
            Field = field;
            UIControl = uiControl;
            IsMultiSelect = isMultiSelect;
            //Item = new SelectListItem() { Text = field, Value = ruleFieldId.ToString()};
        }

        #region Properties
        [DataMember]
        public int RuleFieldId { get; set; }
        [DataMember]
        public string DataTable { get; set; }
        [DataMember]
        public string Field { get; set; }
        [DataMember]
        public string UIControl { get; set; }
        [DataMember]
        public bool IsMultiSelect { get; set; }
        #endregion
        //[DataMember]
        //SelectListItem Item { get { return new SelectListItem() { Text = Field, Value = RuleFieldId.ToString() }; }
        //    set { value = new SelectListItem() { Text = Field, Value = RuleFieldId.ToString() }; }  
    }

}

