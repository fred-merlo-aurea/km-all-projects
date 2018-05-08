using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Common
{
    public class Address1 : TextBox
    {
        public Address1() { }

        public Address1(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Address1";
                LabelHTML = "Address1";
                DataType = TextboxDataTypes.Text;                
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Address1;
            }
        }
    }
}
