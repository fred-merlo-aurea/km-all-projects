using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class Mobile : TextBox
    {
        public Mobile() { }

        public Mobile(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "Mobile";
                LabelHTML = "Mobile";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.Mobile;
            }
        }
    }
}
