using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Common
{
    public class LastName : TextBox
    {
        public LastName() { }

        public LastName(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "LastName";
                LabelHTML = "LastName";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.LastName;
            }
        }
    }
}
