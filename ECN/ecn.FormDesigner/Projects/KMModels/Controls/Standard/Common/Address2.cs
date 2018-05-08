using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Common
{
    public class Address2 : TextBox
    {
        public Address2() { }

        public Address2(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Address2";
                LabelHTML = "Address2";
                DataType = TextboxDataTypes.Text;                
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Address2;
            }
        }
    }
}
