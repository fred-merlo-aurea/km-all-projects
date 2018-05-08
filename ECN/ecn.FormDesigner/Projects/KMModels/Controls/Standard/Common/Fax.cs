using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Common
{
    public class Fax : TextBox
    {
        public Fax() { }

        public Fax(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Fax";
                LabelHTML = "Fax";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Fax;
            }
        }
    }
}
