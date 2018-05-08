using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class Income : TextBox
    {
        public Income() { }

        public Income(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Income";
                LabelHTML = "Income";
                DataType = TextboxDataTypes.Number;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Income;
            }
        }
    }
}
