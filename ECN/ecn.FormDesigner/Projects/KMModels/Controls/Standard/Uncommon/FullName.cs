using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class FullName : TextBox
    {
        public FullName() { }

        public FullName(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "FullName";
                LabelHTML = "FullName";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.FullName;
            }
        }
    }
}
