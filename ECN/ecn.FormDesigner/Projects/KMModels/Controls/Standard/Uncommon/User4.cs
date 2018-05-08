using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class User4 : TextBox
    {
        public User4() { }

        public User4(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "User4";
                LabelHTML = "User4";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.User4;
            }
        }
    }
}
