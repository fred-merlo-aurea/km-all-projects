using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Common
{
    public class Phone : TextBox
    {
        public Phone() { }

        public Phone(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Phone";
                LabelHTML = "Phone";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Phone;
            }
        }
    }
}
