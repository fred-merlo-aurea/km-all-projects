using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Common
{
    public class FirstName : TextBox
    {
        public FirstName() { }

        public FirstName(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "FirstName";
                LabelHTML = "FirstName";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.FirstName;
            }
        }
    }
}
