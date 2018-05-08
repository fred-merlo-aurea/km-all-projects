using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls
{
    public class ListItem : ModelBase
    {
        public ListItem() { }

        public ListItem(bool setDefaultValues = false) 
        {
            if (setDefaultValues)
            {
                Text = "Default";
                Value = "Default";
            }
        }

        [GetFromField("FormControlPropertyGrid_Seq_ID")]
        public int Id { get; set; }

        [GetFromField("DataText")]
        public string Text { get; set; }

        [GetFromField("DataValue")]
        public string Value { get; set; }

        [GetFromField("IsDefault")]
        public bool Default { get; set; }

        [GetFromField("CategoryID")]
        public int CategoryID { get; set; }

        [GetFromField("CategoryName")]
        public string CategoryName { get; set; }

        [GetFromField("Order")]
        public int Order { get; set; }
    }
}
