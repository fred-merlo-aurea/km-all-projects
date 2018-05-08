using KMEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMModels.Controls.Standard.Uncommon
{
    public class User6 : TextBox
    {
        public User6() { }

        public User6(bool setDefaultValues = false)
        {
            if (setDefaultValues)
            {
                Label = "User6";
                LabelHTML = "User6";
                DataType = TextboxDataTypes.Text;
            }
        }

        public override ControlType Type
        {
            get
            {
                return ControlType.User6;
            }
        }
    }
}
